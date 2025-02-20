using AutoMapper;
using Book_Management_API.DataAccess;
using Book_Management_API.DTOs;
using Book_Management_API.Models;
using Microsoft.AspNetCore.Mvc;


namespace Book_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound("book not found");
            }
            return Ok(book);
        }
        [HttpGet]
        public async Task<ActionResult<BookDTO>> GetByTitle(string title)
        {
            var book = await _bookRepository.GetBookByTitle(title);
            if (book == null)
            {
                return NotFound("book not found");
            }
            return Ok(book);
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetAllTitle()
        {
            var bookstitle = await _bookRepository.GetBooksTitle();
            return Ok(bookstitle);
        }
        [HttpPost]
        public async Task<IActionResult> Insert(List<BookDTO> books)
        {
            var map = _mapper.Map<List<Book>>(books);
            if (books.Count == 1)
            {
                
                var id = await _bookRepository.AddBook(map[0]);
                if(id == -1)
                {
                    return BadRequest("publication year is invalid");
                }
                else if(id == -2)
                {
                    return BadRequest("Book already exists");
                }
                return Ok(id);
            }
            else
            {
                var ids = await _bookRepository.AddBulkBooks(map);
                return Ok(ids);
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update(BookDTO book)
        {
            var bookmap = _mapper.Map<Book>(book);
            await _bookRepository.UpdateBook(bookmap);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            if(ids.Count == 1)
            {
                await _bookRepository.DeleteBook(ids[0]);
                return Ok();
            }
            else
            {
                await _bookRepository.DeleteBulkBooks(ids);
                return Ok();
            }
        }
    }
}
