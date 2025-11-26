using Data8.PowerPlatform.Dataverse.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchidpharmed.Medical.AdverseEventAPI.IHelper;
using System.Collections.Concurrent;


[Route("api/[controller]")]
[ApiController]
public class AuditController : ControllerBase
{
    private readonly Helper _helper;
    private IOrganizationService OrgService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuditController> _logger;

    public AuditController(IConfiguration configuration, ILogger<AuditController> logger)
    {
        _logger = logger;
        _configuration = configuration;
        var crmServiceUrl = _configuration["CRMServiceUrl"];
        try
        {
            OrgService = new OnPremiseClient(crmServiceUrl,
            _configuration["CRMUsername"],
            _configuration["CRMPassword"]);
            _helper = new Helper(OrgService);
        }
        catch (Exception e)
        {
            _logger.LogInformation($"Error - {DateTime.Now.ToLongTimeString()} - Error while trying to connect to CRM. Details: {e.Message}");
        }
    }

    [HttpGet("GetOpportunityAudit")]
    public async Task<IActionResult> GetOpportunityAudit(DateTime startDate, DateTime endDate, int pageNumber = 1)
    {
        try
        {
            var query = new QueryExpression("audit")
            {
                ColumnSet = new ColumnSet(true),
                PageInfo = new PagingInfo
                {
                    PageNumber = pageNumber,
                    Count = 50 // Adjust the page size as needed
                }
            };

            // Filter by date range
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrAfter, startDate);
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrBefore, endDate);


            var objectTypeCodeFilter = new FilterExpression(LogicalOperator.Or);
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "opportunity");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "new_productorder");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "new_productdonation");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "op_moratoriumtime");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "new_otherfocgift");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "new_productordertracking");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "new_opportunitycontact");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "op_opportunityletter");
            objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, "op_attachment");

            query.Criteria.AddFilter(objectTypeCodeFilter);



            var results = new List<Entity>();
            EntityCollection response;

            response = OrgService.RetrieveMultiple(query);
            results.AddRange(response.Entities);

            var auditHistory = new ConcurrentBag<JObject>();

            Parallel.ForEach(results, entity =>
            {
                try
                {
                    EntityReference entityRef = entity.GetAttributeValue<EntityReference>("objectid");
                    var attributeMask = entity.GetAttributeValue<string>("attributemask");
                    var attributeNames = DecryptAttributeMask(attributeMask, entityRef.LogicalName);

                    Parallel.ForEach(attributeNames, attributeName =>
                    {
                        try
                        {
                            var history = GetAuditHistoryForField(entityRef.LogicalName, entityRef.Id, attributeName, entity.Id, true);
                            auditHistory.Add(history);
                        }
                        catch (Exception)
                        {
                            // Log or handle the exception as needed
                        }
                    });
                }
                catch (Exception)
                {
                    // Log or handle the exception as needed
                }
            });

            var result = new
            {
                Audit = JArray.FromObject(auditHistory),//auditHistory.Select(j => j.ToObject<object>()).ToList(),
                PageNumber = pageNumber,
                NextPage = response.MoreRecords
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving audit records: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("GetAudit")]
    public async Task<IActionResult> GetAudit(DateTime startDate, DateTime endDate, string tableName, int pageNumber = 1)
    {
        try
        {
            var query = new QueryExpression("audit")
            {
                ColumnSet = new ColumnSet(true),
                PageInfo = new PagingInfo
                {
                    PageNumber = pageNumber,
                    Count = 50 // Adjust the page size as needed
                }
            };

            // Filter by date range
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrAfter, startDate);
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrBefore, endDate);

            // Filter by object type codes
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table Name can not be null");
            }
            else
            {
                var objectTypeCodeFilter = new FilterExpression(LogicalOperator.And);
                objectTypeCodeFilter.AddCondition("objecttypecode", ConditionOperator.Equal, tableName);
                query.Criteria.AddFilter(objectTypeCodeFilter);
            }


            var results = new List<Entity>();
            EntityCollection response;

            response = OrgService.RetrieveMultiple(query);
            results.AddRange(response.Entities);

            var auditHistory = new ConcurrentBag<JObject>();

            Parallel.ForEach(results, entity =>
            {
                try
                {
                    EntityReference entityRef = entity.GetAttributeValue<EntityReference>("objectid");
                    var attributeMask = entity.GetAttributeValue<string>("attributemask");
                    var attributeNames = DecryptAttributeMask(attributeMask, entityRef.LogicalName);

                    Parallel.ForEach(attributeNames, attributeName =>
                    {
                        try
                        {
                            var history = GetAuditHistoryForField(entityRef.LogicalName, entityRef.Id, attributeName, entity.Id, false);
                            auditHistory.Add(history);
                        }
                        catch (Exception)
                        {
                            // Log or handle the exception as needed
                        }
                    });
                }
                catch (Exception)
                {
                    // Log or handle the exception as needed
                }
            });

            var result = new
            {
                Audit = JArray.FromObject(auditHistory),//auditHistory.Select(j => j.ToObject<object>()).ToList(),
                PageNumber = pageNumber,
                NextPage = response.MoreRecords
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving audit records: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    private List<string> DecryptAttributeMask(string attributeMask, string entityLogicalName)
    {
        var attributeNames = new List<string>();
        var attributeBits = attributeMask.Split(',');

        foreach (var bit in attributeBits)
        {
            var attributeName = GetAttributeNameByIndex(entityLogicalName, bit);
            attributeNames.Add(attributeName);
        }

        return attributeNames;
    }

    private string GetAttributeNameByIndex(string entityLogicalName, string attributeIndex)
    {
        var request = new RetrieveEntityRequest
        {
            EntityFilters = EntityFilters.Attributes,
            LogicalName = entityLogicalName
        };

        var response = (RetrieveEntityResponse)OrgService.Execute(request);
        try
        {
            var attributeMetadata = response.EntityMetadata.Attributes.FirstOrDefault(a => a.ColumnNumber == int.Parse(attributeIndex));
            return attributeMetadata?.LogicalName ?? string.Empty;
        }
        catch (Exception)
        {

            return string.Empty;
        }


    }

    private string GetNumber(string entityName, Guid id)
    {
        Entity mainEntity;
        EntityReference relatedEntity;
        switch (entityName)
        {
            case "opportunity":
                mainEntity = _helper.Retrieve(entityName, id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "new_productorder":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("new_opportunity")).GetAttributeValue<EntityReference>("new_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "new_productdonation":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("new_opportunity")).GetAttributeValue<EntityReference>("new_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "op_moratoriumtime":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("op_opportunity")).GetAttributeValue<EntityReference>("op_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "new_otherfocgift":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("new_opportunity")).GetAttributeValue<EntityReference>("new_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "new_productordertracking":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("new_opportunity")).GetAttributeValue<EntityReference>("new_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "new_opportunitycontact":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("new_opportunity")).GetAttributeValue<EntityReference>("new_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "op_opportunityletter":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("op_opportunity")).GetAttributeValue<EntityReference>("op_opportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "op_attachment":
                relatedEntity = _helper.Retrieve(entityName, id, new ColumnSet("op_parentopportunity")).GetAttributeValue<EntityReference>("op_parentopportunity");
                mainEntity = _helper.Retrieve(relatedEntity.LogicalName, relatedEntity.Id, new ColumnSet("new_opportunitynumber"));
                return mainEntity.Contains("new_opportunitynumber") ? (string)mainEntity["new_opportunitynumber"] : string.Empty;
            case "incident":
                mainEntity = _helper.Retrieve(entityName, id, new ColumnSet("ticketnumber"));
                return mainEntity.Contains("ticketnumber") ? (string)mainEntity["ticketnumber"] : string.Empty;
            default:
                return string.Empty;
        }
    }

    private string GetAttributeValue(Entity entity, string attributeName)
    {
        if (entity.Contains(attributeName))
        {
            var attributeValue = entity[attributeName];
            switch (attributeValue)
            {
                case OptionSetValue optionSetValue:
                    return $"{GetOptionSetLabel(entity.LogicalName, attributeName, optionSetValue.Value)} ({optionSetValue.Value})";
                case EntityReference entityReference:
                    return $"{entityReference.Name}";
                case Money money:
                    return money.Value.ToString("NO");
                case DateTime dateTime:
                    return dateTime.ToString("o");
                case bool boolean:
                    return boolean.ToString();
                case int integer:
                    return integer.ToString();
                case string str:
                    return str;
                case decimal dec:
                    return dec.ToString();
                case double dbl:
                    return dbl.ToString();
                case Guid guid:
                    return guid.ToString();
                case OptionSetValueCollection optionSetValueCollection:
                    return string.Join(", ", optionSetValueCollection.Select(v => $"{GetOptionSetLabel(entity.LogicalName, attributeName, v.Value)}"));
                default:
                    return attributeValue.ToString();
            }
        }
        return string.Empty;
    }

    private string GetOptionSetLabel(string entityLogicalName, string attributeLogicalName, int optionSetValue)
    {
        var request = new RetrieveAttributeRequest
        {
            EntityLogicalName = entityLogicalName,
            LogicalName = attributeLogicalName,
            RetrieveAsIfPublished = true
        };

        var response = (RetrieveAttributeResponse)OrgService.Execute(request);
        if (response.AttributeMetadata is PicklistAttributeMetadata picklistMetadata)
        {
            var option = picklistMetadata.OptionSet.Options.FirstOrDefault(o => o.Value == optionSetValue);
            return option != null ? option.Label.UserLocalizedLabel.Label : optionSetValue.ToString();
        }
        else if (response.AttributeMetadata is MultiSelectPicklistAttributeMetadata multiSelectMetadata)
        {
            var option = multiSelectMetadata.OptionSet.Options.FirstOrDefault(o => o.Value == optionSetValue);
            return option != null ? option.Label.UserLocalizedLabel.Label : optionSetValue.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    private string GetAttributeLabel(string entityLogicalName, string attributeLogicalName)
    {
        var request = new RetrieveAttributeRequest
        {
            EntityLogicalName = entityLogicalName,
            LogicalName = attributeLogicalName,
            RetrieveAsIfPublished = true
        };

        var response = (RetrieveAttributeResponse)OrgService.Execute(request);
        return response.AttributeMetadata.DisplayName.UserLocalizedLabel.Label;
    }

    private JObject GetAuditHistoryForField(string entityName, Guid entityId, string attributeName, Guid auditId, bool isForOpportunity)
    {
        var number = GetNumber(entityName, entityId);
        if (string.IsNullOrEmpty(number) && isForOpportunity)
            return new JObject();

        if (string.IsNullOrEmpty(attributeName))
            return new JObject();

        var request = new RetrieveAttributeChangeHistoryRequest
        {
            Target = new EntityReference(entityName, entityId),
            AttributeLogicalName = attributeName
        };

        var response = (RetrieveAttributeChangeHistoryResponse)OrgService.Execute(request);
        var auditDetails = response.AuditDetailCollection.AuditDetails;

        var auditEntry = new JObject();
        foreach (var detail in auditDetails)
        {
            try
            {
                var attributeAuditDetail = (AttributeAuditDetail)detail;
                if (attributeAuditDetail.AuditRecord.Id == auditId)
                {
                    var oldValue = GetAttributeValue(attributeAuditDetail.OldValue, attributeName);
                    var newValue = GetAttributeValue(attributeAuditDetail.NewValue, attributeName);
                    var changeDate = attributeAuditDetail.AuditRecord.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("MM-dd-yyyy HH:mm:ss");
                    var user = attributeAuditDetail.AuditRecord.GetAttributeValue<EntityReference>("userid").Name;
                    var operation = attributeAuditDetail.AuditRecord.GetAttributeValue<OptionSetValue>("operation").Value;
                    var operationName = GetOptionSetLabel("audit", "operation", operation);
                    //var action = attributeAuditDetail.AuditRecord.GetAttributeValue<OptionSetValue>("action").Value;
                    //var actionName = GetOptionSetLabel("audit", "action", action);

                    //var persianCalendar = new System.Globalization.PersianCalendar();
                    //var persianDate = $"{persianCalendar.GetYear(changeDate)}/{persianCalendar.GetMonth(changeDate):00}/{persianCalendar.GetDayOfMonth(changeDate):00} {changeDate:HH:mm:ss}";

                    var attributeLabel = GetAttributeLabel(entityName, attributeName);

                    auditEntry = new JObject
                    {
                        ["Table Name"] = entityName,
                        ["Record Id"] = entityId,
                        ["Event"] = operationName,
                        ["Change Time"] = changeDate,
                        ["Changed Field"] = attributeLabel,
                        ["Changed By"] = user,
                        ["Old Value"] = oldValue,
                        ["New Value"] = newValue
                    };
                    if (isForOpportunity)
                    {
                        auditEntry["Order Id"] = number;
                    }
                    else
                    {
                        auditEntry["Id"] = number;
                    }
                    break;
                }
            }
            catch (Exception)
            {

                continue;
            }

        }

        return auditEntry;
    }
}

