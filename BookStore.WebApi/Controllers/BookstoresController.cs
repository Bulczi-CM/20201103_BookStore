using BookStore.BusinessLayer;
using BookStore.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.WebApi.Controllers
{
    [Route("api/bookstores")]
    public class BookstoresController : ControllerBase
    {
        private readonly IBookStoreService _bookStoreService;

        public BookstoresController(IBookStoreService bookStoreService)
        {
            _bookStoreService = bookStoreService;
        }

        [HttpPost]
        public async Task PostBookstore([FromBody] Bookstore bookstore)
        {
            await _bookStoreService.AddAsync(bookstore);
        }

        [HttpPost("addbook")]
        public async Task PostBookToBookStore([FromBody] BookStoreBook bookStoreBook)
        {
            await _bookStoreService.AddBookToBookStoreAsync(bookStoreBook);
        }
    }
}
