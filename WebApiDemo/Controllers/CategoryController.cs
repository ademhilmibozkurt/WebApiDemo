using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;

namespace WebApiDemo.Controllers
{   
    // include cross origin requests
    [EnableCors]
    // this is an api controller
    [ApiController]
    // route api/ conroller name
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ProductContext _context;
        public CategoryController(ProductContext context)
        {
            _context = context;
        }

        // api way catch category id /then products in this category
        [HttpGet("{id}/products")]
        public IActionResult GetWithProducts(int id)
        {
            var data = _context.Categories.Include(x => x.Products).SingleOrDefault(x => x.Id == id);

            if (data == null)
                return NotFound(id);

            return Ok(data);
        }
    }
}
