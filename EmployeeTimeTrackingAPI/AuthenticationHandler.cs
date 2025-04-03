using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Encodings.Web;

namespace EmployeeTimeTrackingAPI
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("No authorization header."));
            }

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header."));
            }

            var encodedCredentials = authorizationHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            var credentials = decodedCredentials.Split(':');
            if (credentials.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format."));
            }

            var username = credentials[0];
            var password = credentials[1];

            // Weryfikacja użytkownika w prostym przykładzie
            if (username == "admin" && password == "password")
            {
                var claims = new[] { new System.Security.Claims.Claim("username", username) };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, "Basic");
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "Basic");

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            // W przypadku niepoprawnych danych logowania, zwróć błąd
            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
        }
    }
}
