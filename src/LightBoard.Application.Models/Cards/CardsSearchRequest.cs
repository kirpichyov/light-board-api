namespace LightBoard.Application.Models.Cards
{
    public class CardsSearchRequest
    {
        public string Text { get; set; }
        public bool? SearchInTitle { get; set; }
        public bool? SearchInDescription { get; set; }
    }
}
