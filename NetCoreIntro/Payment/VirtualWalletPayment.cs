namespace NetCoreIntro.Payment
{
  public class VirtualWalletPayment:IPayment
  {
    private readonly ILogger<VirtualWalletPayment> _logger;
    public VirtualWalletPayment(ILogger<VirtualWalletPayment> logger)
    {
      _logger = logger;
    }
    public void Pay(decimal amount, string currency)
    {
      _logger.LogInformation($"Processing virtual wallet payment of {amount} {currency}.");
    }

  }
}
