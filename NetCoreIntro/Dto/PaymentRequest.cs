namespace NetCoreIntro.Dto
{
  public record PaymentRequest(decimal Amount, string Currency, string Type)
  {
  }
}
