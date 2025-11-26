using System;

namespace Dynamics365.Merge.Common
{
    public class MergeError
    {
        public int? RowNumber { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public string Stage { get; set; }
        public string Operation { get; set; }
        public string RelationshipSchemaName { get; set; }
        public Guid? RelatedRecordId { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

