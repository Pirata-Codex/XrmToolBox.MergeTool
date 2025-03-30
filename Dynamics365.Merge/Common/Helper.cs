using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace Dynamics365.Merge.Common
{
    internal class Helper
    {
        private readonly IOrganizationService _service;

        public Helper(IOrganizationService service)
        {
            _service = service;
        }

        private static readonly Dictionary<string, int> DefaultStatusCodes = new Dictionary<string, int>
    {
        { "account", 2 },       // Inactive
        { "contact", 2 },       // Inactive
        { "opportunity", 5 },   // Closed (Lost)
        { "lead", 2 },          // Disqualified
        { "incident", 2 },      // Resolved
        { "quote", 4 },         // Closed
        { "salesorder", 4 },    // Fulfilled
        { "invoice", 3 }        // Closed
    };

        public void DeactivateRecord(string entityName, Guid recordId)
        {
            if (string.IsNullOrEmpty(entityName) || recordId == Guid.Empty)
            {
                throw new ArgumentException("Entity name and record ID are required.");
            }

            int statusCode = GetStatusCodeForEntity(entityName);

            // Use UpdateRequest to deactivate the record
            Entity entity = new Entity(entityName)
            {
                Id = recordId
            };
            entity["statecode"] = new OptionSetValue(1);  // 1 = Inactive
            entity["statuscode"] = new OptionSetValue(statusCode);

            _service.Update(entity);
        }

        private int GetStatusCodeForEntity(string entityName)
        {
            if (DefaultStatusCodes.TryGetValue(entityName, out int statusCode))
            {
                return statusCode;
            }

            // Retrieve metadata dynamically for custom entities
            RetrieveEntityRequest request = new RetrieveEntityRequest
            {
                LogicalName = entityName,
                EntityFilters = EntityFilters.Attributes
            };

            var response = (RetrieveEntityResponse)_service.Execute(request);
            var statusAttribute = response.EntityMetadata.Attributes
                .FirstOrDefault(a => a.LogicalName == "statuscode") as StatusAttributeMetadata;

            if (statusAttribute != null && statusAttribute.OptionSet != null)
            {
                // Default to the first inactive status found
                var inactiveOption = statusAttribute.OptionSet.Options
                    .FirstOrDefault(o => o.Value != 1); // Assuming 1 is Active

                if (inactiveOption != null)
                {
                    return inactiveOption.Value.Value;
                }
            }

            return 2; // Default generic inactive status code
        }
    }
}
