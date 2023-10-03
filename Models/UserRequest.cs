using System.ComponentModel.DataAnnotations;

namespace PortalWeb_API.Models
{
    public class UserRequest
    {
        [Required]
        public string? Usuario { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
