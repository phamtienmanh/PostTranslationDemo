using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Contexts;
using WebApplication1.Enums;
using WebApplication1.Models;
using WebApplication1.Repostories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostTranslationController : ControllerBase
    {
        private readonly IAppDbRepository _appRepo;
        private readonly IMapper _mapper;

        public PostTranslationController(IAppDbRepository appRepo, IMapper mapper)
        {
            _appRepo = appRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetPostById")]
        public IActionResult GetPostTranslationById(long id)
        {
            var post = _appRepo.GetPostTranslation(id);
            if (post is null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet]
        public IActionResult GetPostsTranslationByLanguage(LanguageCode language = LanguageCode.en)
        {
            var postsInLanguage = _appRepo.GetPostTranslationByLanguage(language);
            return Ok(postsInLanguage.ToList());
        }

        [HttpPost]
        public IActionResult CreatePostTranslationByLanguage([FromBody] PostTranslationCreate postTranslation)
        {
            if (postTranslation.PostId is null)
            {
                var newPost = new Post();
                _appRepo.CreatePost(newPost);
                _appRepo.Save();
                postTranslation.PostId = newPost.Id;
            }
            else
            {
                if (!_appRepo.IsPublishedAndExistPost((long)postTranslation.PostId))
                {
                    ModelState.AddModelError("Description", "Post is not exist or unpublished!");
                    return BadRequest(ModelState);
                }
            }
            var newPostTranslation = _mapper.Map<PostTranslation>(postTranslation);
            _appRepo.CreatePostTranslation(newPostTranslation);
            _appRepo.Save();
            var created = _mapper.Map<PostTranslationCreate>(newPostTranslation);
            return CreatedAtRoute("GetPostById", 
                new { id = created.Id },
                created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePostTranslationById(long id, [FromBody] PostTranslationUpdate postTranslation)
        {
            var updatePostTranslation = _appRepo.GetPostTranslation(id);
            if (updatePostTranslation is null)
            {
                return NotFound();
            }

            _mapper.Map(postTranslation, updatePostTranslation);
            _appRepo.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchUpdatePostTranslationById(long id, [FromBody] JsonPatchDocument<PostTranslationUpdate> postTranslation)
        {
            var updatePostTranslation = _appRepo.GetPostTranslation(id);
            if (updatePostTranslation is null)
            {
                return NotFound();
            }

            var patchUpdatePostTranslation = _mapper.Map<PostTranslationUpdate>(updatePostTranslation);
            postTranslation.ApplyTo(patchUpdatePostTranslation);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _mapper.Map(patchUpdatePostTranslation, updatePostTranslation);
            _appRepo.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePostTranslationById(long id)
        {
            var deletePostTranslation = _appRepo.GetPostTranslation(id);
            if (deletePostTranslation is null)
            {
                return NotFound();
            }

            _appRepo.DeletePostTranslation(deletePostTranslation);
            _appRepo.Save();
            return NoContent();
        }
    }
}
