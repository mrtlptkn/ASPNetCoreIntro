using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreIntro.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SecuresController : ControllerBase
  {
    private readonly ILogger<SecuresController> _logger;

    public SecuresController(ILogger<SecuresController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IActionResult Get([FromHeader] string XAPIKEY)
    {
      _logger.LogInformation("Secures Controller -> " + XAPIKEY);

      return Ok(new { message = "Bu endpoint'e erişmek için geçerli bir API key'e sahip olmanız gerekiyor." });
    }
  }
}
