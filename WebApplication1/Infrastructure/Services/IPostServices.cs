using System.Collections.Generic;
using WebApplication1.DataAccess.Entities;
using WebApplication1.Infrastructure.Models;

namespace WebApplication1.Infrastructure.Services
{
    public interface IPostServices
    {
        Post GetPostById(long id, bool isIncludeTrans = false);
        PostModel GetPostModelById(long id, string languageCode);
        IEnumerable<PostModel> GetPosts(string languageCode);
        PostModel CreatePost(PostModel postModel);
        PostModel CreateTranslation(Post post, PostModel postModel);
        bool UpdatePost(long id, PostModel postModel);
        bool DeletePost(long id);
        bool DeleteTranslate(long id, string languageCode);
    }
}
