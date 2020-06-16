using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Contexts;
using WebApplication1.Enums;
using WebApplication1.Repostories;

namespace WebApplication1.Repositories
{
    public class AppDbRepository: IAppDbRepository
    {
        private readonly AppDbContext _dbContext;

        public AppDbRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }
        public bool IsPublishedAndExistPost(long id)
        {
            return _dbContext.Posts.Any(x => x.Published && x.Id == id);
        }

        public bool IsExistPostTranslation(long id)
        {
            return _dbContext.PostTranslations.Any(x => x.Id == id);
        }

        public Post GetPublishedPost(long id)
        {
            return _dbContext.Posts.FirstOrDefault(x => x.Published && x.Id == id);
        }

        public PostTranslation GetPostTranslation(long id)
        {
            return _dbContext.PostTranslations.FirstOrDefault(x => x.Id == id && x.Post.Published);
        }

        public void CreatePost(Post post)
        {
            _dbContext.Posts.Add(post);
        }

        public void CreatePostTranslation(PostTranslation postTranslation)
        {
            _dbContext.PostTranslations.Add(postTranslation);
        }

        public void DeletePostTranslation(PostTranslation postTranslation)
        {
            _dbContext.PostTranslations.Remove(postTranslation);
        }

        public IEnumerable<PostTranslation> GetPostTranslationByLanguage(LanguageCode languageCode)
        {
            return _dbContext.PostTranslations.Where(x => x.LanguageCode == languageCode.ToString() && x.Post.Published)
                .OrderByDescending(x => x.Post.CreatedDate);
        }
    }
}
