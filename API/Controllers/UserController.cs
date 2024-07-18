using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs.Requests;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;

        [HttpGet]
        [Authorize(Policy = "MinimalAge")]
        public ActionResult GetUser()
        {
            return Ok(_signInManager.UserManager.Users.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> PostUserAsync([FromBody]UserRequest userRequest)
        {
            var user = Entities.User.Create(
                userRequest.Name ?? string.Empty, 
                userRequest.Email ?? string.Empty, 
                userRequest.BirthDate 
                );

            var response = await _userManager.CreateAsync(user, userRequest.Password!);
            
            return Created(string.Empty, response);
        }

        [HttpPost]
        [Route("logins")]
        public async Task<ActionResult> PostLogin([FromBody]LoginRequest loginRequest)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginRequest.UserName ?? string.Empty, 
                    loginRequest.Password ?? string.Empty, 
                    false, 
                    false);
            
                if (result.Succeeded)
                {
                    var user = _signInManager
                        .UserManager
                        .Users
                        .ToList()
                        .First(
                            u => string.Equals(
                                u.NormalizedUserName, 
                                loginRequest.UserName, 
                                StringComparison.OrdinalIgnoreCase));
                
                    var claims = new[]
                    {
                        new Claim("id", user.Id),
                        new Claim("username", loginRequest.UserName!),
                        new Claim("birthdata", user.BirthDate.ToString(CultureInfo.InvariantCulture)),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("s5gly78n4g5t941ws5gly78n4g5t941w"));
                    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signingCredentials);

                    var response = new JwtSecurityTokenHandler().WriteToken(token);
                
                    return Ok(response);
                }

                return BadRequest("Invalid Login.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
