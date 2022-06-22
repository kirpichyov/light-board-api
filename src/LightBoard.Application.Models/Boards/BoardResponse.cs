using LightBoard.Application.Models.Columns;

namespace LightBoard.Application.Models.Boards;

public class BoardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyCollection<ColumnResponse> Columns { get; set; }
    public string BackgroundUrl { get; set; }
}