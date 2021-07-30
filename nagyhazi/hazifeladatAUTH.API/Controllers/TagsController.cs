using hazifeladat.BLL.DTOs;
using hazifeladat.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hazifeladat.API.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }
        // GET: api/<TagsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
        {
            return (await _tagService.GetTagsAsync()).ToList();
        }

        // GET api/<TagsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> Get(int id)
        {
            return await _tagService.GetTagAsync(id);
        }

        // POST api/<TagsController>
        [HttpPost]
        public async Task<ActionResult<Tag>> Post([FromBody] Tag tag)
        {
            var created = await _tagService.InsertTagAsync(tag);
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        created);
        }

        // PUT api/<TagsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tag tag)
        {
            await _tagService.UpdateTagAsync(id, tag);
            return NoContent();
        }

        // DELETE api/<TagsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return NoContent();
        }
    }
}
