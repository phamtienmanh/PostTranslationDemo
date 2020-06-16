using System.ComponentModel.DataAnnotations;
using WebApplication1.Enums;

namespace WebApplication1.Models
{
    public class PostTranslationCreate: PostTranslationUpdate
    {
        public long? Id { get; set; }
        public long? PostId { get; set; }
    }
    public class PostTranslationUpdate
    {
        public LanguageCode? LanguageCode { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
