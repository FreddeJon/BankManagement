using Application.Features.Api.Command.AuthenticateLogin;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public class LoginModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        var response = await _mediator.Send(new AuthenticateLoginCommand() { AuthenticationRequest = new AuthenticationRequest() { Email = login.Email, Password = login.Password } });

        if (response.Status == Application.Responses.StatusCode.Error)
        {
            return Unauthorized(response.StatusText);
        }

        return Ok(new { Status = response.StatusText, Token = response.Token });
    }
}