using GeneratorApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorApi.Api
{
    [ApiController]
    [ApiResultFilter]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
