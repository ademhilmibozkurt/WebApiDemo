using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using WebApiDemo.Data;
using WebApiDemo.Interfaces;

namespace WebApiDemo.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        //// api/products
        //[HttpGet] 
        //public IActionResult GetProducts()
        //{
        //    return Ok(new[] { new {Name="MSI Creator Z16", Price="65000" }, new {Name="Asus ZenPhone X", Price="23000" } });
        //}

        //// api/products/{id}
        //[HttpGet("{id}")]
        //public IActionResult GetProduct(int id) 
        //{
        //    return Ok(new {Name="Asus ROG Zephyrus", Price="780000"});
        //}

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =  await _productRepository.GetAllAsync();
            return Ok(result);
        }

        // api/products?id=1, [HttpGet("getById")], [FromQuery]
        // api/products/1, [HttpGet("{id}")], [FromRoute] - default
        [Authorize(Roles =  "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _productRepository.GetByIdAsync(id);

            if(data == null) 
                return NotFound();

            return Ok(data);
        }

        // api/products?id=1&name=bilgisayar&price=..... [FromQuery]
        // before (!Product product) -> ! = [FromBody] - default
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var addedProduct = await _productRepository.CreateAsync(product);
            return Created(string.Empty, addedProduct);
        }

        // before (!Product product) -> ! = [FromBody] - default
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var check = await _productRepository.GetByIdAsync(product.Id);

            if (check == null)
                return NotFound(product.Id);
            
            await _productRepository.UpdateAsync(product);
            return NoContent();
        }

        // before (!int id) -> ! = [FromRoute] - default 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var check = await _productRepository.GetByIdAsync(id);

            if (check == null)
                return NotFound(id);

            await _productRepository.RemoveAsync(id);
            return NoContent();
        }

        // api/products/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]IFormFile formFile)
        {
            var newName = Guid.NewGuid() + "." + Path.GetExtension(formFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newName);
            var stream = new FileStream(path, FileMode.Create);
            
            await formFile.CopyToAsync(stream);
            return Created(string.Empty, formFile);
        }
    }
}
