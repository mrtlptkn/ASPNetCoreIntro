using NetCoreIntro.Service;

namespace NetCoreIntro.Middlewares
{
  public class ExternalServiceInjectionMiddleware
  {
    private readonly RequestDelegate _next;

    public ExternalServiceInjectionMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    // InvokeAsync method is called for each HTTP request, TestService is injected automatically by the DI container

    public async Task InvokeAsync(HttpContext context, TestService externalService)
    {

      // TestService externalService -> Araya Service injection yapıldı. Bu sayede middleware içerisinde TestService sınıfının Handle metodunu çağırabiliriz.

      externalService.Handle();

      // Middleware logic here
      // You can use the injected externalService to perform operations
      // Call the next middleware in the pipeline
      await _next(context);
    }
  }
}
