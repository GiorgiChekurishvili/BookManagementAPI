using AutoMapper;
using Book_Management_API.DataAccess;
using Book_Management_API.DTOs;
using Book_Management_API.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound("book not found");
            }

            var mapbook = _mapper.Map<BookDTO>(book);
            return Ok(mapbook);
        }
        [Authorize]
        [HttpGet("by-title")]
        public async Task<ActionResult<BookDTO>> GetByTitle([FromQuery] string title)
        {
            var book = await _bookRepository.GetBookByTitle(title);
            if (book == null)
            {
                return NotFound("book not found");
            }
            var mapbook = _mapper.Map<BookDTO>(book);
            return Ok(mapbook);
        }
        [Authorize]
        [HttpGet("titles")]
        public async Task<ActionResult<string>> GetAllTitle()
        {
            var bookstitle = await _bookRepository.GetBooksTitle();
            return Ok(bookstitle);
        }
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Insert([FromBody] BookDTO book)
        {
            var map = _mapper.Map<Book>(book);
            var id = await _bookRepository.AddBook(map);
            if (id == -1)
            {
                return BadRequest("Book already exists");
            }
            else if (id == -2)
            {
                return BadRequest("publication year is invalid");
            }
            return Ok(id);
       
        }
        [Authorize]
        [HttpPost("add/bulk")]
        public async Task<IActionResult> InsertBulk([FromBody] List<BookDTO> books)
        {
            var map = _mapper.Map<List<Book>>(books);
            var ids = await _bookRepository.AddBulkBooks(map);
            return Ok(ids);

        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDTO book)
        {
            
            var bookmap = _mapper.Map<Book>(book);
            bookmap.Id = id;
            var validations = await _bookRepository.UpdateBook(bookmap);
            if(validations == null)
            {
                return BadRequest("either id or publication year is invalid or changed book title already exists");
            }
            return Ok();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookRepository.DeleteBook(id);
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteBulk(List<int> ids)
        {
            await _bookRepository.DeleteBulkBooks(ids);
            return NoContent();
            
        }
    }
}
