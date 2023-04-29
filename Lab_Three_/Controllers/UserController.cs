using Lab_Three_.Data.Models;
using Lab_Three_.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab_Three_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Employee> _userManager;

        public UserController(IConfiguration configuration, UserManager<Employee> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        #region Static Login
        [HttpPost]
        [Route("Static-Login")]
        public ActionResult<TokenDto> StaticLogin(LoginDto credentials)
        {
            if (credentials.Username != "admin" || credentials.Password != "password")
            {
                return BadRequest();
            }
            var claimList = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, "123456"),
                new Claim(ClaimTypes.Name, credentials.Username),
                new Claim (ClaimTypes.Role,"Employee"),
                new Claim ("Nationality","Egyptian"),

            };
            var keyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
            var keyInBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expiry = DateTime.Now.AddMinutes(15);
            var jwt = new JwtSecurityToken
                (
               expires: expiry,
                claims: claimList,
                signingCredentials: signingCredentials
                );
            //Getting Token String
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);
            return new TokenDto { Token = tokenString };
        }
        #endregion

        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
        {
            Employee? user = await _userManager.FindByNameAsync(credentials.Username);
            if (user == null)
            {
                //we can send a message
                return BadRequest();
            }
            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, credentials.Password);
            if (!isPasswordCorrect)
            {
                return BadRequest();
            }

            var claimsList = await _userManager.GetClaimsAsync(user);
            var keyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
            var keyInBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expiry = DateTime.Now.AddMinutes(15);
            var jwt = new JwtSecurityToken
                (
               expires: expiry,
                claims: claimsList,
                signingCredentials: signingCredentials
                );
            //Getting Token String
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);
            return new TokenDto { Token = tokenString,Expiry = expiry };
        }
        #endregion

        #region  User Register
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
        {
            var newEmployee =
                new Employee
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    Department = registerDto.Department,
                };
            var createResult = await _userManager.CreateAsync(newEmployee, registerDto.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);

            }
            var claims = new List<Claim>
            {
                 new Claim (ClaimTypes.NameIdentifier,newEmployee.Id),
                 new Claim (ClaimTypes.Role,"User")
                 

              };
            await _userManager.AddClaimsAsync(newEmployee, claims);
            return NoContent();
        }
        #endregion

        #region Admin Register
        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<ActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            var newEmployee =
                new Employee
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    Department = registerDto.Department,
                };
            var createResult = await _userManager.CreateAsync(newEmployee, registerDto.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);

            }
            var claims = new List<Claim>
            {
                 new Claim (ClaimTypes.NameIdentifier,newEmployee.Id),
                 new Claim (ClaimTypes.Role,"User")


              };
            await _userManager.AddClaimsAsync(newEmployee, claims);
            return NoContent();
        }
        #endregion

    }
}
