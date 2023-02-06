using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "GeneralRight")]
    public class IdentityController : ControllerBase
    {
        [HttpGet("identity")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new {c.ValueType, c.Value});
        }
    }
}
