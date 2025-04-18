using JWTIdentityApi.Entities;
using JWTIdentityApi.Models;
using JWTIdentityApi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JWTIdentityApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser>? _userManager;
        private readonly RoleManager<AppRole>? _roleManager;
        private readonly IAuthenticationService _authService;

        public UsersController(UserManager<AppUser>? userManager, IAuthenticationService authService, RoleManager<AppRole>? roleManager)
        {
            _userManager = userManager;
            _authService = authService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel model)
        {
            var user = new AppUser()
            {
                Name = model.Name,
                LastName=model.LastName,
                UserName=model.Username,
                Email=model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, errorMessage: error.Description);
                    return Ok(ModelState);
                }
            }
            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                var role = new AppRole()
                {
                    Name="Admin"
                };
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user,"Admin");
            return Ok("User successfully Registered");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if(user is null)
            {
                return Unauthorized();
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return BadRequest("Wrong Password");
            var token = await _authService.CreateToken(user);
            return Ok(token);
        }
    }
}
