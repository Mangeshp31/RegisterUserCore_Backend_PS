using Microsoft.AspNetCore.Identity;
using RegisterUserCore.Authentication;
using RegisterUserCore.Repositories.Interface;
using Microsoft.IdentityModel.Tokens;
using RegisterUserCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace RegisterUserCore.Repositories.Implimentation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthRepository(UserManager<ApplicationUser> userManager, IConfiguration configuration,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        /// <summary>
        /// implementation for registration method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(RegisterModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                IsActive = model.IsActive = 1,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,

            };

            return await _userManager.CreateAsync(user, model.Password);
        }



        /// <summary>
        /// implementation of Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,false,false);
            if (!result.Succeeded)
            {
                return null;
            }

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

                    );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
