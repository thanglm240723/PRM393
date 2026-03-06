using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        [MaxLength(50)]
        public string? Genre { get; set; }

        [Range(1, 99999, ErrorMessage = "Số trang phải từ 1 đến 99999")]
        public int? PageCount { get; set; }

        [Range(1000, 2100, ErrorMessage = "Năm xuất bản không hợp lệ")]
        public int? PublishedYear { get; set; }

        [Range(0.0, 5.0, ErrorMessage = "Rating phải từ 0 đến 5")]
        public decimal? Rating { get; set; }

        [MaxLength(20)]
        public string? Language { get; set; }

        public string? FileUrl { get; set; }


        public List<CreateChapterRequest>? Chapters { get; set; }
    }


    public class CreateChapterRequest
    {
        [Required]
        [Range(1, 9999)]
        public int ChapterNumber { get; set; }

        [MaxLength(255)]
        public string? ChapterTitle { get; set; }

        [Required(ErrorMessage = "Nội dung chương không được để trống")]
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateBookRequest
    {
        [MaxLength(255)]
        public string? Title { get; set; }

        [MaxLength(100)]
        public string? Author { get; set; }

        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }

        [MaxLength(50)]
        public string? Genre { get; set; }

        [Range(1, 99999)]
        public int? PageCount { get; set; }

        [Range(1000, 2100)]
        public int? PublishedYear { get; set; }

        [Range(0.0, 5.0)]
        public decimal? Rating { get; set; }

        [MaxLength(20)]
        public string? Language { get; set; }

        public string? FileUrl { get; set; }
    }


    public class CreateBookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int ChaptersAdded { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
