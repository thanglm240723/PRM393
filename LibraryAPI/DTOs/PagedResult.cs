namespace LibraryAPI.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; } // Tổng số bản ghi trong DB
        public int PageNumber { get; set; } // Trang hiện tại
        public int PageSize { get; set; }   // Kích thước trang
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize); // Tổng số trang
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }
}