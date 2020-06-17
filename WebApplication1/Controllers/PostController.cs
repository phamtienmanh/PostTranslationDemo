using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataAccess.Contexts;
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

        public PostController(AppDbContext dbContext, IMapper mapper)
        {
            _postServices = new PostServices(dbContext, mapper);
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
                new { id = createdPostModel.PostId, languageCode = createdPostModel.LanguageCode },
                createdPostModel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] PostModel postModel, [FromQuery] string languageCode = LanguageCode.English)
        {
            if (!_postServices.UpdatePost(id, languageCode, postModel))
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

        [HttpDelete]
        public IActionResult DeleteTranslation([FromQuery] long translationId)
        {
            if (!_postServices.DeleteTranslate(translationId))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
