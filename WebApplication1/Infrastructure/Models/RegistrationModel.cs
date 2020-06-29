using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Infrastructure.Models
{
    public class RegistrationModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
