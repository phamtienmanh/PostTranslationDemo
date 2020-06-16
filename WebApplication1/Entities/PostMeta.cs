using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.Json.Serialization;
using WebApplication1.Enums;

namespace WebApplication1
{
    public class PostMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long PostId { get; set; }
        [ForeignKey("PostId")]
        [JsonIgnore]
        public Post Post { get; set; }
        [Required]
        [MaxLength(20)]
        public string LanguageCode { get; set; }
        [MaxLength(150)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}