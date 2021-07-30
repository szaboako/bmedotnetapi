using hazifeladat.BLL.DTOs;
using hazifeladat.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hazifeladat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        // GET: api/<ArticleController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> Get()
        {
            return (await _articleService.GetArticlesAsync()).ToList();
        }

        // GET api/<ArticleController>/?id=5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> Get(int id)
        {
            return await _articleService.GetArticleAsync(id);
        }

        [HttpGet("{tag}")]
        public async Task<ActionResult<IEnumerable<Article>>> Get(string tag)
        {
            return (await _articleService.GetArticlesByTagAsync(tag)).ToList();
        }

        // POST api/<ArticleController>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Article>> Post([FromBody] Article article)
        {
            var created = await _articleService.InsertArticleAsync(article);
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        created);
        }

        // PUT api/<ArticleController>/5
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Article article)
        {
            await _articleService.UpdateArticleAsync(id, article);
            return NoContent();
        }

        // DELETE api/<ArticleController>/5
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}
