using Microsoft.AspNetCore.Identity;

namespace IdentityCourse.Models
{
    public class AppUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}
