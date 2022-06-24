using LightBoard.Shared.Contracts;
using LightBoard.Shared.Models.Enums;
using Newtonsoft.Json;

namespace LightBoard.Application.Models.Records;

public class HistoryRecordArgs<TItem>
{
    public Guid UserId { get; set; }
    public Guid ResourceId { get; set; }
    public ResourceType ResourceType { get; set; }
    public DateTime CreatedTime { get; set; }
    public ActionType ActionType { get; set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    
    public void SetOldValue(IPureCloneable cloneable)
    {
        OldValue = JsonConvert.SerializeObject(cloneable.GetPureObject());
    } 
    
    public void SetNewValue(IPureCloneable cloneable)
    {
        NewValue = JsonConvert.SerializeObject(cloneable.GetPureObject());
    } 
}