namespace LightBoard.DataAccess.Abstractions.Arguments
{
    public class SearchCardsArgs
    {
        public string Text { get; set; }
        public bool? SearchInTitle { get; set; }
        public bool? SearchInDescription { get; set; }
    }
}
