using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class UserLoginDTOsModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
