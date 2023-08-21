using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers
{
    [Authorize]
    [Route("API/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    { }
}
