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
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureService _pictureService;
        public PicturesController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }
        // GET: api/<PicturesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> Get()
        {
            return (await _pictureService.GetPicturesAsync()).ToList();
        }

        // GET api/<PicturesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> Get(int id)
        {
            return await _pictureService.GetPictureAsync(id);
        }

        // POST api/<PicturesController>
        [HttpPost]
        public async Task<ActionResult<Article>> Post(string description,[FromForm] IFormFile pic)
        {
            var created = await _pictureService.InsertPictureAsync(description, pic);
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        created);
        }

        // PUT api/<PicturesController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, string description, [FromForm] IFormFile pic)
        {
            await _pictureService.UpdatePictureAsync(id, description, pic);
            return NoContent();
        }

        // DELETE api/<PicturesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pictureService.DeletePictureAsync(id);
            return NoContent();
        }
    }
}
