namespace UnitTest.FakeServices;
public class FakeUserService : IUserService
{
    public string? GetCurrentUser()
    {
        return "Test";
    }
}
