using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Contexts;
using WebApplication1.DataAccess.Entities;
using WebApplication1.Infrastructure.Enums;

namespace WebApplication1.DataAccess.Repositories
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

        public PostTranslation GetPostById(long id)
        {
            return _dbContext.PostTranslations.Include(x => x.Post)
                .FirstOrDefault(x => x.Post.Published && x.PostId == id && x.LanguageCode == LanguageCode.en.ToString());
        }

        public IEnumerable<PostTranslation> GetPosts()
        {
            return _dbContext.PostTranslations.Include(x => x.Post)
                .Where(x => x.Post.Published && x.LanguageCode == LanguageCode.en.ToString());
        }

        public void CreatePost(Post post)
        {
            _dbContext.Posts.Add(post);
        }

        public void DeletePost(Post post)
        {
            _dbContext.Posts.Remove(post);
        }
    }
}
