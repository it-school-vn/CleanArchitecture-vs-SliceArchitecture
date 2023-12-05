namespace CleanArchitecture.Domain.Models
{
    public class Pagination<T>
    {
        public Pagination(IEnumerable<T> data, int count, int page, int pageSize)
        {
            Data = data;
            CurrentPage = page;
            PageSize = pageSize;
            TotalCount = count;
        }

        public IEnumerable<T> Data { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : TotalCount;

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;



    }
}