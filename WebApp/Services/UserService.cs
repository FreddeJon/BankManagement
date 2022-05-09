using System.Security.Claims;
using Persistence.Contracts;

namespace WebApp.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentUser()
    {

        var user = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        return string.IsNullOrEmpty(user) ? "N/A" : user;
    }
}