using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Api.Command.AuthenticateLogin;
public class AuthenticateLoginCommandHandler : IRequestHandler<AuthenticateLoginCommand, AuthenticateLoginResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthenticateLoginCommandHandler(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthenticateLoginResponse> Handle(AuthenticateLoginCommand request, CancellationToken cancellationToken)
    {
        var response = new AuthenticateLoginResponse();
        var user = await _userManager.FindByEmailAsync(request.AuthenticationRequest.Email);
        if (user is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Email or password is incorrect";
            return response;
        }

        var correctPassword = await _userManager.CheckPasswordAsync(user, request.AuthenticationRequest.Password);
        if (!correctPassword)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Email or password is incorrect";
            return response;
        }

        var claims = GetClaimsForUser(user);
        var generatedSecurityToken = CreateToken(claims);


        response.Token = new JwtSecurityTokenHandler().WriteToken(generatedSecurityToken);
        return response;
    }


    private JwtSecurityToken CreateToken(List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        return new JwtSecurityToken(
            issuer: _jwtOptions.ValidIssuer,
            audience: _jwtOptions.ValidAudience,
            expires: DateTime.Now.AddMinutes(_jwtOptions.TokenValidityInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        ); ;
    }

    private List<Claim> GetClaimsForUser(IdentityUser user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        return authClaims;
    }
}




