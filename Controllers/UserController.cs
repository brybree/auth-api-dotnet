using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController
    {
        private IUserService object1;
        private ITokenService object2;

        public UserController(IUserService object1, ITokenService object2)
        {
            this.object1 = object1;
            this.object2 = object2;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register registerModel)
        {
            throw new NotImplementedException();
        }
    }
}