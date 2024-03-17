using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login()
        {
            // call jwttokengenerator. in every login take one identical jwt token
            return Created("", new JwtTokenGenerator().GenerateToken());
        }
    }
}
