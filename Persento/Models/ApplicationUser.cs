using Microsoft.AspNetCore.Identity;

namespace Persento.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Fullname { get; set; }
        public bool IsActivated { get; set; }
    }
}
