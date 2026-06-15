using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreIntro.Service;

namespace NetCoreIntro.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TestsController : ControllerBase
  {
    // Dependency Injection (DI) ile service instance controllera enjekte ediliyor.
    // private readonly TestService testService = new TestService(); tight coupled kullanım

    // TestService instance sistem tarafında otomatik al. TestsController bunu yönetmesin.
    // Bunu yöneten yapısa ise IoC (Inversion of Control) Container. Bu sayede TestService instance yönetimi IoC Container tarafından yapılır.
    private readonly TestService testService1;
    private readonly TestService testService2;

    public TestsController(TestService testService1, TestService testService2)
    {
      this.testService1 = testService1;
      this.testService2 = testService2;

    }



    [HttpGet]
    public IActionResult Get()
    {

      this.testService1.Handle();
      this.testService2.Handle();

      // service instace controllerdan ulaşma yöntemi

      return Ok("Hello from TestsController!");
    }


  }
}
