using LightBoard.Shared.Contracts;
using LightBoard.Shared.Models.Enums;

namespace LightBoard.Application.Models.Records;

public class HistoryRecordResponse
{
    public Guid UserId { get; set; }
    public Guid ResourceId { get; set; }
    public ResourceType ResourceType { get; set; }
    public DateTime CreatedTime { get; set; }
    public ActionType ActionType { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}