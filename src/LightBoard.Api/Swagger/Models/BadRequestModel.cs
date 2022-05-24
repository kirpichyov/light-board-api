namespace LightBoard.Api.Swagger.Models;

public class BadRequestModel
{
    public ErrorNode[] Errors { get; set; }
    
    public class ErrorNode
    {
        public string Property { get; set; }
        public string Error { get; set; }
    }
}