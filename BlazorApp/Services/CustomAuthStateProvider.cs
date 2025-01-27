using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
           // var user = new ClaimsPrincipal(new ClaimsIdentity());// non-autheticated

           var claims = new List<Claim> { new Claim(ClaimTypes.Name, "John") };
           var identity = new ClaimsIdentity(claims, "Any");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
