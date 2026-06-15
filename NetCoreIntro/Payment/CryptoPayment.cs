namespace NetCoreIntro.Payment
{
  public class CryptoPayment : IPayment
  {
    private readonly ILogger<CryptoPayment> _logger;

    public CryptoPayment(ILogger<CryptoPayment> logger)
    {
      _logger = logger;
    }

    public void Pay(decimal amount, string currency)
    {
      _logger.LogInformation($"Processing crypto payment of {amount} {currency}.");

    }
  }
}
