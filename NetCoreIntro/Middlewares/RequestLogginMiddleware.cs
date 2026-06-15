using System.Diagnostics;

namespace NetCoreIntro.Middlewares
{
  public class RequestLogginMiddleware
  {

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLogginMiddleware> _logger;
    public RequestLogginMiddleware(RequestDelegate next, ILogger<RequestLogginMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    // InvokeAsync özel bir isim, dikkat edelim.
    public async Task InvokeAsync(HttpContext context)
    {

      // Senaryo istek giriş ve çıkışı arasında ms metrik değeri bulmak
      Stopwatch sp = Stopwatch.StartNew();


      var method = context.Request.Method;
      var path = context.Request.Path;

      try
      {
        // Log the incoming request
        //Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");

        _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Incoming request Error: {context.Request.Method} {context.Request.Path} - {ex.Message}");
       
      }
      finally
      {
        sp.Stop();
        _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} completed in {sp.ElapsedMilliseconds} ms");
      }


      var logLevel = context.Response.StatusCode switch
      {
        >= 500 => "Error",
        >= 400 => "Warning",
        _ => "Information"
      };

      _logger.Log(logLevel switch
      {
        "Error" => LogLevel.Error,
        "Warning" => LogLevel.Warning,
        _ => LogLevel.Information
      }, $"Response: {context.Response.StatusCode} for {context.Request.Method} {context.Request.Path}");

      // bir sonraki middleware süreci aktar

      // artık response dönmüş durumda, response loglama işlemi yapabiliriz. cevap aşamasında tekrar loglanır.


    }
  }
}
