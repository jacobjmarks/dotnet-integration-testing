using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Example.Api.Tests;

public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
}

public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";

    public TestAuthenticationHandler(
        IOptionsMonitor<TestAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = Request.GetClaims();
        if (!claims.Any())
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(claimsPrincipal, AuthenticationScheme);
        var authenticationResult = AuthenticateResult.Success(ticket);
        return Task.FromResult(authenticationResult);
    }
}
