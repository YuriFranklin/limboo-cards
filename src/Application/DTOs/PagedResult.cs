namespace LimbooCards.Application.DTOs
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public bool HasNextPage { get; init; }
        public bool HasPreviousPage { get; init; }
    }
}