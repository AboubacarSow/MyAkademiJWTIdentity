using JWTIdentityApi.Entities;
using JWTIdentityApi.JwtConfig;
using JWTIdentityApi.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTIdentityApi.Services.Models
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtTokenOptions? _tokenOptions;
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationService(IOptions<JwtTokenOptions> token, UserManager<AppUser> userManager)
        {
            _tokenOptions = token.Value;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser user)
        {
            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.Key));
            var userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Role,userRoles.FirstOrDefault()!),
                new Claim("FullName",String.Join(" ",user.Name,user.LastName))
            };
            JwtSecurityToken securityToken = new(
                issuer:_tokenOptions.Issuer,
                audience:_tokenOptions.Audience,
                claims:claims,
                notBefore:DateTime.Now,
                expires:DateTime.Now.AddMinutes(_tokenOptions.ExpiresIn),
                signingCredentials: new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
