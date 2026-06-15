namespace NetCoreIntro.Middlewares
{
  // Güvenlik gerekçesi ile uygulama gelen api isteklerinde bazı security headaer bilgisi olup olmadığını kontrol etmek için middleware yazıyoruz.
  public class ApiKeyMiddleware
  {
    private readonly RequestDelegate _next;
    private const string API_KEY_HEADER_NAME = "XAPIKEY";
    private readonly IConfiguration _configuration; // Middleware içerisinde config dosyası okumak için IConfiguration kullanıyoruz.
    private readonly ILogger<ApiKeyMiddleware> _logger;


    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
      _next = next;
      _configuration = configuration;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      // Json Web Token mantığı bu tarz bir middleware ile tüm istekleri veya belirli başlı endpointelere olan istekleri yönetiyor.

      if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
      {
        // Headerdan key gönderilmedi 401 Unauthorized hatası dönüyoruz.
        context.Response.StatusCode = 401; // Unauthorized
        await context.Response.WriteAsJsonAsync(new { message = "API Key is missing" });
        return;
      }
      // Burada API key doğrulama işlemi yapılabilir. Örneğin, bir konfigürasyon dosyasında saklanan geçerli API key ile karşılaştırabilirsiniz.
      var appSettingsApiKey = _configuration.GetValue<string>("ApiKey:Key"); // Bu değeri güvenli bir şekilde saklamalısınız.
      if (String.IsNullOrEmpty(appSettingsApiKey) || !appSettingsApiKey.Equals(extractedApiKey))
      {
        // Key var ama eşleşmiyorsa, 403 forbidden hatası dönüyoruz.
        context.Response.StatusCode = 403; // Forbidden
        await context.Response.WriteAsJsonAsync(new { message = "Unauthorized client" });
        return;
      }


      this._logger.LogInformation("Api Secure is completed -> middleware");

      await _next(context);
    }



  }


  public static class ApiKeyMiddlewareExtensions
  {
    // Middleware'i uygulama pipeline'ına eklemek için bir extension method yazıyoruz.
    public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ApiKeyMiddleware>();
    }


  }
}
