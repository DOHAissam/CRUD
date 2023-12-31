using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _service = service;
            Console.WriteLine("User Id: " + User.FindFirstValue(ClaimTypes.NameIdentifier));
            Console.WriteLine("Username: " + User.FindFirstValue(ClaimTypes.Name));

        }
    

  
    }
}
