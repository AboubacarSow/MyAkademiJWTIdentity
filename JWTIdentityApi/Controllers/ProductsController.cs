using JWTIdentityApi.Repositories.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace JWTIdentityApi.Controllers
{
    [ApiController]
    [Route("api/products")]

    public class ProductsController : ControllerBase
    {
        private readonly RepositoryContext? _context;

        public ProductsController(RepositoryContext? context)
        {
            _context = context;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("allProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var models =await  _context.Products.ToListAsync();
            return Ok(models);
        }
    }
}
