namespace NetCoreIntro.Payment
{
  public interface IPayment
  {
    void Pay(decimal amount, string currency);
  }
}
