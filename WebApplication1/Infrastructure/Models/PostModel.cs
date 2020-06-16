using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Infrastructure.Models
{
    public class PostModel
    {
        public long? Id { get; set; }
        public long? PostId { get; set; }
        public bool Published { get; set; } = true;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        [Required]
        [MaxLength(20)]
        public string LanguageCode { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
