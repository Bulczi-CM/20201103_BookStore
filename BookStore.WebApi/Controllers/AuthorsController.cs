using BookStore.BusinessLayer;
using BookStore.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.WebApi.Controllers
{

    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpPost]
        public async Task PostAuthor([FromBody] Author author)
        {
            await _authorsService.AddAsync(author);
        }

        [HttpPut("{id}")]
        public async Task UppdateAuthor([FromBody] Author author, int id)
        {
            author.Id = id;
            await _authorsService.UpdateAsync(author);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAuthor(int id)
        {
            await _authorsService.DeleteAsync(new Author
            {
                Id = id
            });
        }
    }
}
