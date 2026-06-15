using NetCoreIntro.Middlewares;
using NetCoreIntro.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// uygulamada kullanýlacak olan baðýmlýlýðý girdik. register service to IoC Container. Bu sayede TestService instance yönetimi IoC Container tarafýndan yapýlýr.
// Test Case 1 -> Scoped
builder.Services.AddScoped<TestService>(); // Herhangi bir classdan DI yapýldýðýnda bu sýnýfýn instance 'ý IoC Container ttarafýndan yönet ve bu instance ver.
// Ayný scoped içerisinde TestService tek bir instance alýr.
// istek akýþý biten kadar Test service ayný instance ile çalýþýrsa performanslý olur. 

// yýukarýsý ise uygulama kullanýlacak olan servislerin tanýmlandýðý kýsým, servis injection.


// Test Case 2 -> Singleton
// builder.Services.AddSingleton<TestService>();


// Test Case 3 -> Transient
// Session Yönetimi, Entity Class Her Request newlenecek ise bunu yaptýrmak
// Request bazlý yeniden kontrol edilmesi gereken filter yapýlarý varsa (AuthorizationHandler)
//builder.Services.AddTransient<TestService>();



// middleware buradan sonra yazýlýyor
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// request bazlý bir miidleware için istek baþladðý anda devreye giremli
app.UseMiddleware<RequestLogginMiddleware>();

// Middlewareler 2' ayrýlýr -> use ve when middleware'leri. Use middleware'leri tüm isteklerde devreye girebilirken, when middleware'leri belirli koþullara göre devreye girebilirler. Örneðin, belirli bir route'a gelen isteklerde devreye girebilirler.

// koþullu middleware örneði -> belirli bir route'a gelen isteklerde devreye girecek middleware'ler tanýmlayalým. Örneðin, "/api/secures" ile baþlayan route'lara gelen isteklerde API key doðrulama middleware'ini devreye sokalým.
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/secures"), appBuilder =>
{
  // Belirli bir route'a gelen isteklerde devreye girecek middleware'ler burada tanýmlanýr.
  appBuilder.UseApiKeyMiddleware(); // API key doðrulama middleware'ini belirli bir route'a gelen isteklerde devreye girecek þekilde uygulama pipeline'ýna ekliyoruz.
});



app.Lifetime.ApplicationStopping.Register(() =>
{
  Console.WriteLine("Uygulama kapanýyor...");

  // Temizlik iþlemleri
  // Cache temizleme
  // Mesaj kuyruðu baðlantýlarýný kapatma
  // Açýk dosyalarý kapatma
});

app.Lifetime.ApplicationStopped.Register(() =>
{
  Console.WriteLine("Uygulama tamamen durdu.");
});



// app.UseApiKeyMiddleware(); // API key doðrulama middleware'ini uygulama pipeline'ýna ekliyoruz. Bu middleware, tüm isteklerde API key doðrulamasý yapacak.

app.UseMiddleware<ExternalServiceInjectionMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
