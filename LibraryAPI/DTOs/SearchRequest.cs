namespace LibraryAPI.DTOs
{
    public class SearchRequest
    {

        public string? SearchTerm { get; set; }
        public string? Genre { get; set; }  

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;


    }
}
