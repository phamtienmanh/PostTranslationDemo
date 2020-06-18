using Microsoft.AspNetCore.Mvc;
using WebApplication1.Infrastructure.Enums;
using WebApplication1.Infrastructure.Models;
using WebApplication1.Infrastructure.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostServices _postServices;

        public PostController(PostServices postServices)
        {
            _postServices = postServices;
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(long id, string languageCode = LanguageCode.English)
        {
            var postModel = _postServices.GetPostModelById(id, languageCode);
            if (postModel == null)
            {
                return NotFound();
            }

            return Ok(postModel);
        }

        [HttpGet]
        public IActionResult Get(string languageCode = LanguageCode.English)
        {
            var posts = _postServices.GetPosts(languageCode);
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult Create([FromBody] PostModel postModel)
        {
            var createdPostModel = _postServices.CreatePost(postModel);
            if (createdPostModel == null)
            {
                return NotFound();
            }

            return CreatedAtRoute("GetById",
                new {id = createdPostModel.Id, languageCode = createdPostModel.LanguageCode},
                createdPostModel);
        }

        [HttpPost("{id}/translation")]
        public IActionResult CreateTranslation(long id, [FromBody] PostModel postModel)
        {
            var message = "";
            var createdPostModel = _postServices.CreateTranslation(id, postModel, ref message);
            if (createdPostModel == null)
            {
                if (string.IsNullOrEmpty(message))
                    return NotFound();
                ModelState.AddModelError("error", message);
                return BadRequest(ModelState);
            }
            return CreatedAtRoute("GetById",
                new { id = createdPostModel.Id, languageCode = createdPostModel.LanguageCode },
                createdPostModel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] PostModel postModel)
        {
            if (!_postServices.UpdatePost(id, postModel))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!_postServices.DeletePost(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}/translation")]
        public IActionResult DeleteTranslation(long id, string languageCode = LanguageCode.English)
        {
            if (!_postServices.DeleteTranslate(id, languageCode))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
