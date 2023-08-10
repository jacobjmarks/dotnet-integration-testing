using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Example.Api.Tests;

public static class TestAuthenticationHandlerExtensions
{
    private const string ClaimHeaderName = "x-test-claim";

    public static HttpClient WithClaim(this HttpClient client, Claim claim)
    {
        ArgumentNullException.ThrowIfNull(claim);

        client.DefaultRequestHeaders.Add(ClaimHeaderName, claim.ToBase64String());
        return client;
    }

    public static HttpRequestMessage WithClaim(this HttpRequestMessage request, Claim claim)
    {
        ArgumentNullException.ThrowIfNull(claim);

        request.Headers.Add(ClaimHeaderName, claim.ToBase64String());
        return request;
    }

    public static IEnumerable<Claim> GetClaims(this HttpRequest request)
    {
        if (!request.Headers.TryGetValue(ClaimHeaderName, out var base64Claims))
            base64Claims = new();
        return base64Claims.Select(ToClaim);
    }

    private static string ToBase64String(this Claim claim)
    {
        using var memoryStream = new MemoryStream();
        using var binaryWriter = new BinaryWriter(memoryStream);
        claim.WriteTo(binaryWriter);
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    private static Claim ToClaim(this string base64String)
    {
        var bytes = Convert.FromBase64String(base64String);
        using var memoryStream = new MemoryStream(bytes);
        using var binaryReader = new BinaryReader(memoryStream);
        return new Claim(binaryReader);
    }
}
