using Microsoft.AspNetCore.Identity;
using RegisterUserCore.Authentication;

namespace RegisterUserCore.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateAsync(RegisterModel model);
        Task<string> Login(LoginModel model);
    }
}
