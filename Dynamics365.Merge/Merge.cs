using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk.Messages;
using Dynamics365.Merge.Common;

namespace Dynamics365.Merge
{
    public class MergeRequest
    {
        private string LogicalName;
        private Guid SourceId;
        private Guid TargetId;
        private IOrganizationService OrgService;
        private bool FillNullsOnTargetFromSource;
        private Helper Helper;
        private EntityMetadata MetaData;
        private readonly List<MergeError> _errors = new List<MergeError>();
        private readonly int? _rowNumber;

        public event EventHandler<string> OnFunctionCalled;
        public event EventHandler<MergeProgressDetail> OnProgressChanged;
        public IReadOnlyCollection<MergeError> Errors => _errors.AsReadOnly();

        public MergeRequest(string logicalName, string sourceId, string targetId, IOrganizationService orgService, bool fillNullsOnTargetFromSource, int? rowNumber = null)
        {
            LogicalName = logicalName;
            SourceId = new Guid(sourceId);
            TargetId = new Guid(targetId);
            this.OrgService = orgService;
            FillNullsOnTargetFromSource = fillNullsOnTargetFromSource;
            Helper = new Helper(orgService);
            _rowNumber = rowNumber;
        }

        private void ReportProgress(string stage, string message, string relationship = null, int? current = null, int? total = null, Guid? relatedRecordId = null)
        {
            OnProgressChanged?.Invoke(this, new MergeProgressDetail
            {
                Stage = stage,
                Message = message,
                RelationshipSchemaName = relationship,
                CurrentItem = current,
                TotalItems = total,
                RelatedRecordId = relatedRecordId,
                SourceId = SourceId.ToString(),
                TargetId = TargetId.ToString()
            });
        }

        private void TrackError(string stage, string message, Exception ex, string operation = null, string relationship = null, Guid? relatedRecordId = null)
        {
            _errors.Add(new MergeError
            {
                RowNumber = _rowNumber,
                SourceId = SourceId.ToString(),
                TargetId = TargetId.ToString(),
                Stage = stage,
                Operation = operation,
                RelationshipSchemaName = relationship,
                RelatedRecordId = relatedRecordId,
                Message = message,
                Details = ex?.ToString(),
                Timestamp = DateTime.UtcNow
            });
        }

        private protected void GetEntityMetaData(string logicalName)
        {
            OnFunctionCalled?.Invoke(this, nameof(GetEntityMetaData));

            RetrieveEntityRequest retrieveEntity = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Relationships,
                LogicalName = logicalName
            };

            RetrieveEntityResponse entityResponse = (RetrieveEntityResponse)OrgService.Execute(retrieveEntity);
            this.MetaData = entityResponse.EntityMetadata;
        }

        private protected OneToManyRelationshipMetadata RetrieveOneToManyRelationship(string relationshipName)
        {
            OnFunctionCalled?.Invoke(this, nameof(RetrieveOneToManyRelationship));

            RetrieveRelationshipRequest req = new RetrieveRelationshipRequest
            {
                Name = relationshipName
            };
            RetrieveRelationshipResponse retrieveRelationshipResponse = (RetrieveRelationshipResponse)OrgService.Execute(req);
            OneToManyRelationshipMetadata relationshipMetadata = (OneToManyRelationshipMetadata)retrieveRelationshipResponse.RelationshipMetadata;
            return relationshipMetadata;
        }

        private protected EntityCollection GetRelatedRecordsBasedOnOneToManyRelationshipMetadata(OneToManyRelationshipMetadata relationshipMetadata, Guid parentId)
        {
            OnFunctionCalled?.Invoke(this, nameof(GetRelatedRecordsBasedOnOneToManyRelationshipMetadata));

            string childEntityType = relationshipMetadata.ReferencingEntity;
            string childEntityFieldName = relationshipMetadata.ReferencingAttribute;

            QueryByAttribute querybyattribute = new QueryByAttribute(childEntityType);

            querybyattribute.ColumnSet = new ColumnSet(childEntityFieldName);
            querybyattribute.Attributes.AddRange(childEntityFieldName);
            querybyattribute.Values.AddRange(parentId);

            return OrgService.RetrieveMultiple(querybyattribute);
        }

        private void MergeOneToManyRelationship()
        {
            OnFunctionCalled?.Invoke(this, nameof(MergeOneToManyRelationship));

            int totalRelationships = MetaData.OneToManyRelationships?.Length ?? 0;
            int currentRelationship = 0;

            foreach (var item in MetaData.OneToManyRelationships)
            {
                currentRelationship++;
                ReportProgress(nameof(MergeOneToManyRelationship), $"Processing relationship {item.SchemaName}", item.SchemaName, currentRelationship, totalRelationships);

                try
                {
                    OneToManyRelationshipMetadata relationshipMetadata = RetrieveOneToManyRelationship(item.SchemaName);
                    EntityCollection relatedRecords = GetRelatedRecordsBasedOnOneToManyRelationshipMetadata(relationshipMetadata, SourceId);

                    string referencingAttribute = relationshipMetadata.ReferencingAttribute;
                    //if (!(bool)relationshipMetadata.IsCustomRelationship)
                    //{
                    //    continue;
                    //}
                    foreach (var childEntity in relatedRecords.Entities)
                    {
                        if (relatedRecords.Entities.Count > 1000)
                            break;
                        if (childEntity.Contains(referencingAttribute))
                        {
                            try
                            {
                                childEntity[referencingAttribute] = new EntityReference(LogicalName, TargetId);
                                OrgService.Update(childEntity);
                                ReportProgress(nameof(MergeOneToManyRelationship), $"Updated child record {childEntity.Id}", item.SchemaName, null, null, childEntity.Id);
                            }
                            catch (Exception updateEx)
                            {
                                TrackError(nameof(MergeOneToManyRelationship), $"Failed updating child record {childEntity.Id}", updateEx, "Update", item.SchemaName, childEntity.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TrackError(nameof(MergeOneToManyRelationship), $"Failed processing relationship {item.SchemaName}", ex, "RetrieveRelationship", item.SchemaName);
                    continue;
                }
            }
        }

        private protected EntityCollection RetrieveManyToManyRecords(ManyToManyRelationshipMetadata manyToMany, EntityReference target)
        {
            OnFunctionCalled?.Invoke(this, nameof(RetrieveManyToManyRecords));

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

            return OrgService.RetrieveMultiple(query);
        }

        private protected void AssociateManyToManyEntityRecords(EntityReference entity1, EntityCollection collection, string entityRelationshipName)
        {
            OnFunctionCalled?.Invoke(this, nameof(AssociateManyToManyEntityRecords));

            try
            {
                int total = collection.Entities.Count;
                int current = 0;
                foreach (var entity2 in collection.Entities)
                {
                    current++;
                    AssociateEntitiesRequest request = new AssociateEntitiesRequest();
                    request.Moniker1 = new EntityReference(entity1.LogicalName, entity1.Id);
                    request.Moniker2 = new EntityReference(entity2.LogicalName, entity2.Id);
                    // Set the relationship name to associate on.
                    request.RelationshipName = entityRelationshipName;

                    // Execute the request.
                    OrgService.Execute(request);
                    ReportProgress(nameof(AssociateManyToManyEntityRecords), $"Associated record {entity2.Id}", entityRelationshipName, current, total, entity2.Id);
                }
            }
            catch (Exception e)
            {
                TrackError(nameof(AssociateManyToManyEntityRecords), $"Failed associating relationship {entityRelationshipName}", e, "Associate", entityRelationshipName);
            }
        }

        private void MergeManyToManyRecords()
        {
            OnFunctionCalled?.Invoke(this, nameof(MergeManyToManyRecords));

            int totalRelationships = MetaData.ManyToManyRelationships?.Length ?? 0;
            int currentRelationship = 0;

            foreach (var item in MetaData.ManyToManyRelationships)
            {
                currentRelationship++;
                ReportProgress(nameof(MergeManyToManyRecords), $"Processing many-to-many {item.SchemaName}", item.SchemaName, currentRelationship, totalRelationships);

                try
                {
                    //if (!(bool)item.IsCustomRelationship) continue;
                    EntityCollection records = RetrieveManyToManyRecords(item, new EntityReference(LogicalName, SourceId));
                    AssociateManyToManyEntityRecords(new EntityReference(LogicalName, TargetId), records, item.SchemaName);
                }
                catch (Exception ex)
                {
                    TrackError(nameof(MergeManyToManyRecords), $"Failed processing many-to-many {item.SchemaName}", ex, "Associate", item.SchemaName);
                    continue;
                }
            }
        }

        private Entity GetEntity(Guid guid)
        {
            QueryExpression source = new QueryExpression(LogicalName);
            source.ColumnSet = new ColumnSet(true);
            source.Criteria.AddCondition(LogicalName + "id", ConditionOperator.Equal, guid);
            EntityCollection entity = OrgService.RetrieveMultiple(source);
            if (entity.Entities.Count == 0)
                throw new Exception($"Entity with guid: {guid.ToString()} Not Found");
            return entity.Entities[0];
        }

        //private void DeactivateRecord(Entity entity)
        //{
        //    //StateCode = 1 and StatusCode = 2 for deactivating Account or Contact
        //    SetStateRequest setStateRequest = new SetStateRequest()
        //    {
        //        EntityMoniker = new EntityReference
        //        {
        //            Id = entity.Id,
        //            LogicalName = entity.LogicalName,
        //        },
        //        State = new OptionSetValue(1),
        //        Status = new OptionSetValue(2)
        //    };
        //    OrgService.Execute(setStateRequest);
        //}

        private void MergeFields()
        {
            OnFunctionCalled?.Invoke(this, nameof(MergeFields));

            Entity source;
            Entity target;
            try
            {
                source = GetEntity(this.SourceId);
                target = GetEntity(this.TargetId);
            }
            catch (Exception ex)
            {
                TrackError(nameof(MergeFields), "Failed retrieving source or target entity", ex, "Retrieve");
                return;
            }

            AttributeCollection columns = source.Attributes;
            foreach (var att in columns)
            {
                if (!target.Contains(att.Key) && source.Contains(att.Key))
                {
                    if (FillNullsOnTargetFromSource)
                    {
                        target.Attributes.Add(att.Key, source[att.Key]);
                        ReportProgress(nameof(MergeFields), $"Copied field {att.Key} from source to target");
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            try
            {
                OrgService.Update(target);
            }
            catch (Exception ex)
            {
                TrackError(nameof(MergeFields), "Failed updating target entity", ex, "Update");
            }
            //try
            //{
            //    Helper.DeactivateRecord(source.LogicalName, source.Id);
            //    //DeactivateRecord(source);
            //}
            //catch (Exception e)
            //{
            //    throw new Exception("Record Merged Successfully but deactivation failed. details: " + e);
            //}
        }

        public void DoMerge()
        {
            try
            {
                GetEntityMetaData(LogicalName);
            }
            catch (Exception ex)
            {
                TrackError(nameof(GetEntityMetaData), "Failed retrieving entity metadata", ex, "RetrieveMetadata");
                return;
            }

            MergeOneToManyRelationship();
            MergeManyToManyRecords();
            MergeFields();
        }
    }
}