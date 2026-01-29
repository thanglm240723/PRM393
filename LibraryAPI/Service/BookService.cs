using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Service
{
    public class BookService : IBookService
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly PersonalLibraryContext _context;
      public BookService(IMapper mapper, IConfiguration configuration, PersonalLibraryContext context)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }



        public async Task<PagedResult<BookResponse>> GetBooksAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <1 ) pageNumber = 1;
            if (pageSize <1 ) pageSize = 10;

            // Tạo trước , chưa thực thi vào db 
            var query = _context.Books.AsQueryable();

            var totalCount = await  query.CountAsync();

            var books = await query
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var BookDtos = _mapper.Map<List<BookResponse>>(books);
            return new PagedResult<BookResponse>
            {
                Items = BookDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };


        }
    }
}
