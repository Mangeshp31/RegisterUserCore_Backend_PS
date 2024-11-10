using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RegisterUserCore.Authentication;
using RegisterUserCore.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegisterUserCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;

        //Add DI in program.cs
        private readonly IAuthRepository _authRepository;

        public AuthenticationController(IAuthRepository authRepository,UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _authRepository = authRepository;

        }


        /// <summary>
        /// This Method is responsible for the user registration. will get the data in object formate from user which is type of
        /// RegisterModel and map that object with ApplicationUser, and then add the data in usermanager and return
        /// the Status code.
        /// </summary>
        /// <param name="model">RegisterModel model</param>
        /// <returns>Status Code</returns>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Failed", Message = "User already exsist" });
            //ApplicationUser user = new ApplicationUser()
            //{
            //    Email = model.Email,
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    UserName = model.UserName,
            //    IsActive = model.IsActive = 1,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdatedDate = DateTime.UtcNow,
                
            //};
            var result = await _authRepository.CreateAsync(model);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exsist" });
            }
            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }


        //Login
        /// <summary>
        /// This Method is responsible for the user Login. will get the data in object formate from user which is type of
        /// LoginModel and check that object with ApplicationUser, and if everything is good then return user Details after 
        /// Authentication and authorization.
        /// </summary>
        /// <param name="model">LoginModel model</param>
        /// <returns>User Information </returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authRepository.Login(model);
            if(string.IsNullOrEmpty(result))
            {
                return Unauthorized();      
            }

            return Ok(result);

        }
    }
}



//public async Task<IActionResult> Login([FromBody] LoginModel model)
//{
//    var user = await userManager.FindByNameAsync(model.Username);
//    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
//    {
//        var userRoles = await userManager.GetRolesAsync(user);
//        var authClaims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, model.Username),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//        foreach (var userRole in userRoles)
//        {
//            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//        }

//        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

//        var token = new JwtSecurityToken(
//            issuer: _configuration["JWT:ValidIssuer"],
//            audience: _configuration["JWT:ValidAudience"],
//            expires: DateTime.Now.AddHours(5),
//            claims: authClaims,
//            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

//            );
//        return Ok(new
//        {
//            token = new JwtSecurityTokenHandler().WriteToken(token),
//            expiration = token.ValidTo,
//            User = user.UserName + " " + "is successfully logged in"
//            //Console.WriteLine("{0} is successfully logged in", User)
//        });

//    }
//    return Unauthorized();
//}