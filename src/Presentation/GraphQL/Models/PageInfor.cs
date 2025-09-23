namespace LimbooCards.Presentation.GraphQL.Models
{
    public class PageInforModel
    {
        public string StartCursor { get; set; } = string.Empty;
        public string EndCursor { get; set; } = string.Empty;
        public bool HasNextPage { get; set; } = false;
        public bool HasPreviousPage { get; set; } = false;
    }
}