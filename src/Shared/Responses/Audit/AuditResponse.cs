using System;

namespace CleanArchTemplate.Shared.Responses.Audit;

public   class AuditResponse
{
    public AuditResponse()
    {
    }

    public AuditResponse(int id, string userId, string type, string tableName, DateTime dateTime, string oldValues,
        string newValues, string affectedColumns, string primaryKey)
    {
        Id = id;
        UserId = userId;
        Type = type;
        TableName = tableName;
        DateTime = dateTime;
        OldValues = oldValues;
        NewValues = newValues;
        AffectedColumns = affectedColumns;
        PrimaryKey = primaryKey;
    }

 

    public int Id { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string AffectedColumns { get; set; }
    public string PrimaryKey { get; set; }
}