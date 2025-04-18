using JWTIdentityApi.Entities;

namespace JWTIdentityApi.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<string> CreateToken(AppUser user);
    }
}
