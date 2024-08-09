using Application.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Infrastructure.Authorization;

public class FakeAuthHandler : AuthenticationHandler<FakeAuthHandlerOptions>
{
    public const string AuthenticationScheme = "Fake";

    public FakeAuthHandler(
        IOptionsMonitor<FakeAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Upn, "fakeuser@gmail.com"),
            new Claim(ClaimTypes.GivenName, "Fake"),
            new Claim(ClaimTypes.Surname, "User"),
            new Claim(ClaimTypes.MobilePhone, "+90 123 456 78 89"),
            new Claim(ClaimTypes.Email, "fakeuser@gmail.com"),
        };

        AddClaimFromHeader(Context, AppConstants.RequestOwnerId, ClaimTypes.NameIdentifier, claims);
        AddClaimFromHeader(Context, AppConstants.Role, ClaimTypes.Role, claims);

        var identity = new ClaimsIdentity(claims, AppConstants.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AppConstants.AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
    private void AddClaimFromHeader(HttpContext context, string headerName, string claimType, ICollection<Claim> claims)
    {
        if (context.Request.Headers.TryGetValue(headerName, out var headerValues))
        {
            claims.Add(new Claim(claimType, headerValues[0]));
        }
    }
}

public class FakeAuthHandlerOptions : AuthenticationSchemeOptions
{
}