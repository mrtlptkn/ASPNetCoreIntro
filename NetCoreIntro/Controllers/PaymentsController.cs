using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreIntro.Dto;
using NetCoreIntro.Payment;

namespace NetCoreIntro.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentsController : ControllerBase
  {

    // PaymentsController -> High level class

    // PaymentController -> IPayment tipindeki nesnelere gideceksen  ozaman CryptoPayment, VirtualWalletPayment bunları bilmeyeceksin sen sadece aradaki IPayement -> intrface yani portu bileceksin.  

    // Dependecy Inversion Prensibine aykırı hareket ettik. 
    // CryptoPayment Low lever class birbirine direkt olarak bağımlı olmamalıdır. Tight coupled. 
    private readonly CryptoPayment _cryptoPayment;
    private readonly VirtualWalletPayment _virtualWalletPayment;
    // Net core tüm servisler servis provider içerisinde bulunuyor. 
    private readonly IServiceProvider _serviceProvider;

    public PaymentsController(CryptoPayment cryptoPayment, VirtualWalletPayment virtualWalletPayment, IServiceProvider serviceProvider)
    {
      _cryptoPayment = cryptoPayment;
      _virtualWalletPayment = virtualWalletPayment;
      _serviceProvider = serviceProvider;
    }


    [HttpPost("pay")]
    public IActionResult Pay([FromBody] PaymentRequest request)
    {
      // IPayment payment = new CryptoPayment();  // High level class, low level classa bağımlı hale geldi. 
      // payment.Pay(100, "USD");
      // IPayment payment = new VirtualWalletPayment();  // High level class, low level classa bağımlı hale geldi. 
      // payment.Pay(100, "USD");
      // Dependency Inversion Prensibine uygun hale getirelim. 
      // service locator pattern. 
      var payment = _serviceProvider.GetRequiredKeyedService<IPayment>(request.Type);

      if (payment == null)
      {
        return BadRequest("Payment service not found.");
      }

      payment.Pay(request.Amount,request.Currency);
      return Ok("Payment processed successfully.");
    }


    [HttpPost("payWithCrypto")]
    public IActionResult PayWithCrypto()
    {
      _cryptoPayment.Pay(100, "USD");
      return Ok("Payment processed successfully.");

    }


    [HttpPost("payWithVirtualWallet")]
    public IActionResult PayWithVirtualWallet()
    {

      _virtualWalletPayment.Pay(100, "USD");
      return Ok("Payment processed successfully.");

    }

  }
}
