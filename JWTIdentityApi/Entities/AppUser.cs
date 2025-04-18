using Microsoft.AspNetCore.Identity;

namespace JWTIdentityApi.Entities
{
    public class AppUser:IdentityUser<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
