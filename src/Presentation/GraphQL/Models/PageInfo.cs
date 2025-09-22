namespace LimbooCards.Presentation.GraphQL.Models
{
    public class PageInfo
    {
        public string? StartCursor { get; set; }
        public string? EndCursor { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}