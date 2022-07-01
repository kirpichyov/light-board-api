using LightBoard.Shared.Models.Enums;

namespace LightBoard.Domain.Entities.Record;

public class ActionHistoryRecord : EntityBase<Guid>
{
    public ActionHistoryRecord(
        Guid userId, 
        Guid resourceId,
        ResourceType resourceType,
        ActionType actionType,
        DateTime createdTime,
        string? oldValue, 
        string? newValue,
        Guid boardId)
    {
        UserId = userId;
        ResourceId = resourceId;
        CreatedTime = createdTime;
        ActionType = actionType;
        ResourceType = resourceType;
        OldValue = oldValue;
        NewValue = newValue;
        BoardId = boardId;
    }
    private ActionHistoryRecord()
    {
    }
    
    public Guid UserId { get; }
    public Guid ResourceId { get; }
    public ResourceType ResourceType { get; }
    public DateTime CreatedTime { get; }
    public ActionType ActionType { get; }
    public string? OldValue { get; }
    public string? NewValue { get; }
    public Guid BoardId { get; }
}