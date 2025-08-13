using System.ComponentModel.DataAnnotations;

namespace Portlink.APP.Models
{
    public class UserModel
    {
        public string CompanyName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
       
      
    }
}
