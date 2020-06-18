using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Infrastructure.Models
{
    public class PostModel
    {
        public long? Id { get; set; }
        public bool Published { get; set; } = true;
        [Required] 
        [MaxLength(20)]
        public string LanguageCode { get; set; } = Enums.LanguageCode.English;
        [MaxLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
