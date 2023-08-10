using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Example.Api.Tests;

public class TestAuthenticationSchemeProvider : AuthenticationSchemeProvider
{
    private readonly AuthenticationScheme _testAuthenticationScheme = new(
        name: TestAuthenticationHandler.AuthenticationScheme,
        displayName: null,
        handlerType: typeof(TestAuthenticationHandler));

    public TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
    {
    }

    public override Task<AuthenticationScheme?> GetSchemeAsync(string name)
    {
        return Task.FromResult((AuthenticationScheme?)_testAuthenticationScheme);
    }
}
