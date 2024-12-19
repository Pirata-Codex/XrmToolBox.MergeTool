using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dynamics365.Merge
{
    public class MergeRequest
    {
        private string LogicalName;
        private Guid SourceId;
        private Guid TargetId;
        private IOrganizationService orgService;
        private bool FillNullsOnTargetFromSource;

        private EntityMetadata MetaData;
        public MergeRequest(string logicalName, string sourceId, string targetId, IOrganizationService orgService, bool fillNullsOnTargetFromSource)
        {
            LogicalName = logicalName;
            SourceId = new Guid(sourceId);
            TargetId = new Guid(targetId);
            this.orgService = orgService;
            FillNullsOnTargetFromSource = fillNullsOnTargetFromSource;
        }

        private protected void GetEntityMetaData(string logicalName)
        {
            RetrieveEntityRequest retrieveEntity = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Relationships,
                LogicalName = logicalName

            };

            RetrieveEntityResponse entityResponse = (RetrieveEntityResponse)orgService.Execute(retrieveEntity);
            this.MetaData = entityResponse.EntityMetadata;
        }

        private protected OneToManyRelationshipMetadata RetrieveOneToManyRelationship(string relationshipName)
        {
            RetrieveRelationshipRequest req = new RetrieveRelationshipRequest
            {
                Name = relationshipName
            };
            RetrieveRelationshipResponse retrieveRelationshipResponse = (RetrieveRelationshipResponse)orgService.Execute(req);
            OneToManyRelationshipMetadata relationshipMetadata = (OneToManyRelationshipMetadata)retrieveRelationshipResponse.RelationshipMetadata;
            return relationshipMetadata;
        }

        private protected EntityCollection GetRelatedRecordsBasedOnOneToManyRelationshipMetadata(OneToManyRelationshipMetadata relationshipMetadata, Guid parentId)
        {
            
            string childEntityType = relationshipMetadata.ReferencingEntity;
            string childEntityFieldName = relationshipMetadata.ReferencingAttribute;

            QueryByAttribute querybyattribute = new QueryByAttribute(childEntityType);

            querybyattribute.ColumnSet = new ColumnSet(childEntityFieldName);
            querybyattribute.Attributes.AddRange(childEntityFieldName);
            querybyattribute.Values.AddRange(parentId);

            return orgService.RetrieveMultiple(querybyattribute);
        }

        private void MergeOneToManyRelationship()
        {
            
            foreach (var item in MetaData.OneToManyRelationships)
            {
                try
                {
                    OneToManyRelationshipMetadata relationshipMetadata = RetrieveOneToManyRelationship(item.SchemaName);
                    EntityCollection relatedRecords = GetRelatedRecordsBasedOnOneToManyRelationshipMetadata(relationshipMetadata, SourceId);

                    string referencingAttribute = relationshipMetadata.ReferencingAttribute;
                    if (!(bool)relationshipMetadata.IsCustomRelationship)
                    {
                        continue;
                    }
                    foreach (var childEntity in relatedRecords.Entities)
                    {
                        if (relatedRecords.Entities.Count > 1000)
                            break;
                        if (childEntity.Contains(referencingAttribute))
                        {
                            childEntity[referencingAttribute] = new EntityReference(LogicalName, TargetId);
                            orgService.Update(childEntity);
                        }
                    }
                }
                catch (Exception e)
                {

                    continue;
                }
                
            }
        }

        private protected EntityCollection RetrieveManyToManyRecords(ManyToManyRelationshipMetadata manyToMany, EntityReference target)
        {
            QueryExpression query = new QueryExpression((manyToMany.Entity1LogicalName == LogicalName ? manyToMany.Entity2LogicalName : manyToMany.Entity1LogicalName));
            LinkEntity linkEntity1 = new LinkEntity
            (
                manyToMany.Entity1LogicalName,
                manyToMany.IntersectEntityName,
                manyToMany.Entity1IntersectAttribute,
                manyToMany.Entity1IntersectAttribute,
                JoinOperator.Inner
            );

            LinkEntity linkEntity2 = new LinkEntity
            (
                manyToMany.IntersectEntityName,
                manyToMany.Entity2LogicalName,
                manyToMany.Entity2IntersectAttribute,
                manyToMany.Entity2IntersectAttribute,
                JoinOperator.Inner
            );


            linkEntity1.LinkEntities.Add(linkEntity2);

            query.LinkEntities.Add(linkEntity1);

            if (manyToMany.Entity1LogicalName == target.LogicalName)
                linkEntity1.LinkCriteria.AddCondition(new ConditionExpression((manyToMany.Entity1LogicalName == LogicalName ? manyToMany.Entity1LogicalName + "id" : manyToMany.Entity2LogicalName), ConditionOperator.Equal, target.Id));
            else if (manyToMany.Entity2LogicalName == target.LogicalName)
                linkEntity1.LinkCriteria.AddCondition(new ConditionExpression((manyToMany.Entity1LogicalName == LogicalName ? manyToMany.Entity1LogicalName : manyToMany.Entity2LogicalName) + "id", ConditionOperator.Equal, target.Id));

            return orgService.RetrieveMultiple(query);
        }
        private protected void AssociateManyToManyEntityRecords(EntityReference entity1, EntityCollection collection, string entityRelationshipName)
        {
            try
            {
                foreach (var entity2 in collection.Entities)
                {
                    AssociateEntitiesRequest request = new AssociateEntitiesRequest();
                    request.Moniker1 = new EntityReference(entity1.LogicalName, entity1.Id);
                    request.Moniker2 = new EntityReference(entity2.LogicalName, entity2.Id);
                    // Set the relationship name to associate on.
                    request.RelationshipName = entityRelationshipName;

                    // Execute the request.
                    orgService.Execute(request);
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        private void MergeManyToManyRecords()
        {
            foreach (var item in MetaData.ManyToManyRelationships)
            {
                try
                {
                    if (!(bool)item.IsCustomRelationship) continue;
                    EntityCollection records = RetrieveManyToManyRecords(item, new EntityReference(LogicalName, SourceId));
                    AssociateManyToManyEntityRecords(new EntityReference(LogicalName, TargetId), records, item.SchemaName);
                }
                catch (Exception e)
                {

                    continue;
                }
                
            }
        }

        private Entity GetEntity(Guid guid)
        {
            QueryExpression source = new QueryExpression(LogicalName);
            source.ColumnSet = new ColumnSet(true);
            source.Criteria.AddCondition(LogicalName+"id", ConditionOperator.Equal, guid);
            EntityCollection entity = orgService.RetrieveMultiple(source);
            return entity.Entities[0];
        }
        private void DeactivateRecord (Entity entity)
        {
            //StateCode = 1 and StatusCode = 2 for deactivating Account or Contact
            SetStateRequest setStateRequest = new SetStateRequest()
            {
                EntityMoniker = new EntityReference
                {
                    Id = entity.Id,
                    LogicalName = entity.LogicalName,
                },
                State = new OptionSetValue(1),
                Status = new OptionSetValue(2)
            };
            orgService.Execute(setStateRequest);
        }
        private void MergeFields()
        {
            Entity source = GetEntity(this.SourceId);
            Entity target = GetEntity(this.TargetId);

            AttributeCollection columns = source.Attributes;
            foreach (var att in columns)
            {

                if (!target.Contains(att.Key) && source.Contains(att.Key))
                {
                    if (FillNullsOnTargetFromSource)
                    {
                        target.Attributes.Add(att.Key, source[att.Key]);
                    }
                    else
                    {
                        continue;
                    }
                }

            }
            DeactivateRecord(source);
            orgService.Update(target);

        }
        public void DoMerge()
        {
            GetEntityMetaData(LogicalName);
            MergeOneToManyRelationship();
            MergeManyToManyRecords();
            MergeFields();

        }

    }
    public class Merge : CodeActivity
    {
        [Input("Logical Name")]
        [RequiredArgument]
        public InArgument<string> SourceLogicalName { get; set; }

        [Input("Source Id")]
        [RequiredArgument]

        public InArgument<string> SourceId { get; set; }

        [Input("Target Id")]
        [RequiredArgument]

        public InArgument<string> TargetId { get; set; }

        private IOrganizationService orgService;

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext workflow = (IWorkflowContext)context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory organizationServiceFactory =
                (IOrganizationServiceFactory)context.GetExtension<IOrganizationServiceFactory>();
            orgService = organizationServiceFactory.CreateOrganizationService(workflow.UserId);

            #region GetParameters
            string LogicalName = context.GetValue(this.SourceLogicalName);
            string sourceId = context.GetValue(this.SourceId);
            string targetId = context.GetValue(this.TargetId);
            #endregion

            MergeRequest merge = new MergeRequest(LogicalName, sourceId, targetId, orgService, true);
            merge.DoMerge();


        }


    }
}

/*
 // Create the target for the request.
            var target = new EntityReference
            {
                // Id is the GUID of the account that is being merged into.
                // LogicalName is the type of the entity being merged to, as a string
                Id = new Guid(context.GetValue(this.SourceId)),
                LogicalName = context.GetValue(this.SourceLogicalName)
            };
            // Create the request.
            var merge = new MergeRequest
            {
                // SubordinateId is the GUID of the account merging.
                SubordinateId = new Guid(context.GetValue(this.TargetId)),
                Target = target,
                PerformParentingChecks = false
            };
            var updateContent = new Entity(context.GetValue(this.SourceLogicalName));
            updateContent.Attributes.Add("trip_address","test");
            merge.UpdateContent = updateContent;


            // Execute the request.
            _ = (MergeResponse)orgService.Execute(merge);
 */