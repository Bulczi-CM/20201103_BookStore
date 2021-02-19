using BookStore.BusinessLayer;
using BookStore.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.WebApi.Controllers
{
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;
        private readonly IAuthorsService _authorsService;

        public BooksController(
            IBooksService booksService,
            IAuthorsService authorsService)
        {
            _booksService = booksService;
            _authorsService = authorsService;
        }

        [HttpPost]
        public async Task PostBook([FromBody] Book book)
        {
            try
            {
                await _booksService.AddBookAsync(book);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [HttpGet]
        public async Task<List<Book>> GetAllBooks()
        {
            var books = await _booksService.GetAllBooksAsync();
            return books;
        }

        //http://localhost:10500/api/books/find?surname=ADFDFDDSFXD&name=pleplepele
        [HttpGet("find")]
        public async Task<List<Book>> GetBooksByAuthor([FromQuery] string surname, [FromQuery] string name)
        {
            var books = await _authorsService.GetBooksByAuthorSurnameAsync(surname);

            return books;
        }
    }
}
