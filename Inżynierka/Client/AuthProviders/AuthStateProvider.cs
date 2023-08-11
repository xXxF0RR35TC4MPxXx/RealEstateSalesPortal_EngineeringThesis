using System.Security.Claims;
using Inżynierka.Shared.DTOs.User;
using Inżynierka_Services.Listing;
using Microsoft.AspNetCore.Components.Authorization;

namespace Inżynierka.Client.AuthProviders;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(claimsPrincipal);
    }

    public void SetAuthInfo(UserDTO userProfile)
    {
        var identity = new ClaimsIdentity(new[]{
            new Claim(ClaimTypes.Name, $"{userProfile.FullName}"),
            new Claim("UserId", userProfile.Id.ToString()) //tu zmienione
        }, "AuthCookie");

        claimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void ClearAuthInfo()
    {
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}