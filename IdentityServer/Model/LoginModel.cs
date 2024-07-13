using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Model
{
    public class LoginModel
    {
        [Required]
        [DefaultValue("user1@example.com")]
        public string EmailOrUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
