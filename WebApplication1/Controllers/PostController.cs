using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataAccess.Entities;
using WebApplication1.DataAccess.Repositories;
using WebApplication1.Infrastructure.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IAppDbRepository _appRepo;
        private readonly IMapper _mapper;

        public PostController(IAppDbRepository appRepo, IMapper mapper)
        {
            _appRepo = appRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetPostById")]
        public IActionResult GetPostById(long id)
        {
            var post = _appRepo.GetPostById(id);
            if (post is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PostModel>(post));
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            var posts = _appRepo.GetPosts();
            return Ok(_mapper.Map<List<PostModel>>(posts));
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] PostModel postModel)
        {
            var newPost = _mapper.Map<Post>(postModel);
            var newPostTrans = _mapper.Map<PostTranslation>(postModel);
            newPost.PostTranslations.Add(newPostTrans);
            _appRepo.CreatePost(newPost);
            _appRepo.Save();
            var createdPost = _mapper.Map<PostModel>(newPostTrans);
            return CreatedAtRoute("GetPostById",
                new { id = createdPost.Id },
                createdPost);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(long id, [FromBody] PostModel postModel)
        {
            var updatePost = _appRepo.GetPostById(id);
            if (updatePost is null)
            {
                return NotFound();
            }
            postModel.CreatedDate = updatePost.Post.CreatedDate;
            postModel.UpdatedDate = DateTime.Now;
            _mapper.Map(postModel, updatePost);
            _mapper.Map(postModel, updatePost.Post);
            _appRepo.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchUpdatePost(long id, [FromBody] JsonPatchDocument<PostModel> postModel)
        {
            var updatePost = _appRepo.GetPostById(id);
            if (updatePost is null)
            {
                return NotFound();
            }

            var patchUpdatePost = _mapper.Map<PostModel>(updatePost);
            postModel.ApplyTo(patchUpdatePost, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _mapper.Map(patchUpdatePost, updatePost);
            _mapper.Map(patchUpdatePost, updatePost.Post);
            _appRepo.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePostTranslationById(long id)
        {
            var deletePost = _appRepo.GetPostById(id);
            if (deletePost is null)
            {
                return NotFound();
            }

            _appRepo.DeletePost(deletePost.Post);
            _appRepo.Save();
            return NoContent();
        }
    }
}
