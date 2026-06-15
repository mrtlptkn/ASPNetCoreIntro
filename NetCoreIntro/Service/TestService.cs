namespace NetCoreIntro.Service
{
  public class TestService
  {
    public string Id { get; init; } 

    public TestService()
    {
      // Service initialization code can go here
      Id  = Guid.NewGuid().ToString();
      
    }


    public void Handle()
    {
      Console.WriteLine($"TestService instance created with Id: {Id}");
      Console.WriteLine("Hello from TestService!");
    }
  }
}
