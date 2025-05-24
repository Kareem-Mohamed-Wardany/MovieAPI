using Microsoft.AspNetCore.Identity;

namespace MovieAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(IdentityUser user, UserManager<IdentityUser> userManager);
    }
}
