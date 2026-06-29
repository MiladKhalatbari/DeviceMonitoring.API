using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers
{
    [Authorize]
    [ApiController]
    public  abstract class BaseController : ControllerBase
    { }
}
