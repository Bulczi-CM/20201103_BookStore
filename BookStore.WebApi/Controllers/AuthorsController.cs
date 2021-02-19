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
    }
}
