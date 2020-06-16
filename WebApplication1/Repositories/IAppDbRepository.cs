using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Enums;

namespace WebApplication1.Repostories
{
    public interface IAppDbRepository
    {
        bool Save();
        bool IsPublishedAndExistPost(long id);
        bool IsExistPostTranslation(long id);
        Post GetPublishedPost(long id);
        PostTranslation GetPostTranslation(long id);
        void CreatePost(Post post);
        void CreatePostTranslation(PostTranslation postTranslation);
        void DeletePostTranslation(PostTranslation postTranslation);
        IEnumerable<PostTranslation> GetPostTranslationByLanguage(LanguageCode languageCode);
    }
}
