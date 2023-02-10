using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Pages.Account.Register
{
    public class registerViewModel
    {
        [Required]

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string RoleName { get; set; }
    }
}
