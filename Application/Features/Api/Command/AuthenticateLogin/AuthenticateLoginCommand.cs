namespace Application.Features.Api.Command.AuthenticateLogin;
public class AuthenticateLoginCommand : IRequest<AuthenticateLoginResponse>
{
    public AuthenticationRequest AuthenticationRequest { get; set; } = null!;
}

public class AuthenticateLoginResponse : BaseResponse
{
    public string? Token { get; set; }
}

public class AuthenticationRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}