using LightBoard.Shared.Contracts;
using LightBoard.Shared.Models.Enums;
using Newtonsoft.Json;

namespace LightBoard.Application.Models.Records;

public class HistoryRecordArgs<TItem>
{
    public Guid UserId { get; init; }
    public Guid ResourceId { get; init; }
    public ResourceType ResourceType { get; init; }
    public DateTime CreatedTime { get; init; }
    public ActionType ActionType { get; init; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public Guid BoardId { get; init; }
    
    public void SetOldValue(IPureCloneable cloneable)
    {
        OldValue = JsonConvert.SerializeObject(cloneable.GetPureObject());
    } 
    
    public void SetNewValue(IPureCloneable cloneable)
    {
        NewValue = JsonConvert.SerializeObject(cloneable.GetPureObject());
    } 
}