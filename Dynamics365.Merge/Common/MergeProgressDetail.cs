using System;

namespace Dynamics365.Merge.Common
{
    public class MergeProgressDetail
    {
        public string Stage { get; set; }
        public string Message { get; set; }
        public string RelationshipSchemaName { get; set; }
        public Guid? RelatedRecordId { get; set; }
        public int? CurrentItem { get; set; }
        public int? TotalItems { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

