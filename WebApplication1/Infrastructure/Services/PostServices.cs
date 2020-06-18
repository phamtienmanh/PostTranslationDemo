using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Contexts;
using WebApplication1.DataAccess.Entities;
using WebApplication1.Infrastructure.Models;

namespace WebApplication1.Infrastructure.Services
{
    public class PostServices
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PostServices(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        private PostTranslation GetTranslationById(long id, string languageCode)
        {
            return _dbContext.PostTranslations.Include(x => x.Post)
                .FirstOrDefault(x => x.PostId == id && x.LanguageCode == languageCode);
        }

        private PostTranslation GetPublishedTranslationById(long id, string languageCode)
        {
            return _dbContext.PostTranslations.Include(x => x.Post)
                .FirstOrDefault(x => x.Post.Published && x.PostId == id && x.LanguageCode == languageCode);
        }

        public PostModel GetPostModelById(long id, string languageCode)
        {
            var postModel = _mapper.Map<PostModel>(GetPublishedTranslationById(id, languageCode));
            return postModel;
        }

        public IEnumerable<PostModel> GetPosts(string languageCode)
        {
            var posts = _dbContext.PostTranslations.Include(x => x.Post)
                .Where(x => x.Post.Published && x.LanguageCode == languageCode);
            return _mapper.Map<IEnumerable<PostModel>>(posts);
        }

        public PostModel CreatePost(PostModel postModel)
        {
            var newPost = _mapper.Map<Post>(postModel);
            newPost.CreatedDate = DateTime.Now;
            _dbContext.Posts.Add(newPost);
            Save();
            return _mapper.Map<PostModel>(newPost.PostTranslations.FirstOrDefault());
        }

        public PostModel CreateTranslation(long id, PostModel postModel, ref string message)
        {
            var post = _dbContext.Posts.FirstOrDefault(x => x.Id == id);
            if (post == null) return null;
            var trans = GetTranslationById(id, postModel.LanguageCode);
            if (trans != null)
            {
                message = $"Translation for {postModel.LanguageCode} is already exist!";
                return null;
            }
            var createdTrans = _mapper.Map<PostTranslation>(postModel);
            post.PostTranslations.Add(createdTrans);

            Save();
            return _mapper.Map<PostModel>(createdTrans); ;
        }

        public bool UpdatePost(long id, PostModel postModel)
        {
            var updatePost = GetTranslationById(id, postModel.LanguageCode);
            if (updatePost is null)
            {
                return false;
            }
            _mapper.Map(postModel, updatePost);
            updatePost.Post.UpdatedDate = DateTime.Now;
            return Save();
        }

        public bool DeletePost(long id)
        {
            var post = _dbContext.Posts.FirstOrDefault(x => x.Id == id);
            if (post == null)
            {
                return false;
            }
            _dbContext.Posts.Remove(post);
            return Save();
        }

        public bool DeleteTranslate(long id, string languageCode)
        {
            var trans = GetTranslationById(id, languageCode);
            if (trans == null)
            {
                return false;
            }
            _dbContext.PostTranslations.Remove(trans);
            Save();
            var post = _dbContext.Posts.Include(x => x.PostTranslations).FirstOrDefault(x => x.Id == id);
            if (post?.PostTranslations.Count == 0)
            {
                _dbContext.Posts.Remove(post);
            }
            return Save();
        }
    }
}
