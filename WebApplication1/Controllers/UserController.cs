using Azure.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication1.Context;
using WebApplication1.Helpers; 
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public UserController(AppDbContext appDbContext) {
            _authContext = appDbContext;

        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();


            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.UserName == userObj.UserName);
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            /*password encrypting and check. have to this because I encrypt password early */

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password)) {

                return BadRequest(new { Message = "Password is Incorrect" });

            }

            /*Create JWT token while auth*/

            user.Token = CreateJWT(user);

            return Ok(new {

                token = user.Token,
                Message = "Login Success!!"
            });

        }

        [HttpPost("register")]

        public async Task<IActionResult> RegisterUser([FromBody] User userObj) {
            if (userObj == null)
                return BadRequest();


            /*  check password*/

            if (await CheckUserNameExistAsync(userObj.UserName))
                return BadRequest(new { Message = "This User Name is Already Exist!!" });


            /*check  email*/

            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "This Email is Already Exist!!" });


            /*check password strength*/

            var pass = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass });



            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";



            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new { Message = "User Registered!!" });
        }

        /*User name check function */
        private Task<bool> CheckUserNameExistAsync(string username)
            => _authContext.Users.AnyAsync(x => x.UserName == username);

        /* Look check password*/


        /*Email checking function */
        private Task<bool> CheckEmailExistAsync(string email)
            => _authContext.Users.AnyAsync(x => x.Email == email);

        /* Look check email*/

        /*Check Password Strength */
        private string CheckPasswordStrength(string password) {

            StringBuilder sb = new StringBuilder();  


            if (password.Length < 8)
                sb.Append("Minimum Password length should be 8" + Environment.NewLine);


            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
                sb.Append("Password Should be Alphanumeric" + Environment.NewLine);


            if (!Regex.IsMatch(password, "[!,<,>,#,%,$,^,&,*,(,),_,+,@,?,/,[,]"))
                sb.Append("Password should be contain special characters" + Environment.NewLine);

            return sb.ToString();


        }

        /* Look password strength*/


        /* Create JWT*/

        private string CreateJWT(User user) {

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("whatisthisokigotit");  
            var identify = new ClaimsIdentity(new Claim[] {  

                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}") 

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = identify,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials   

            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);


        }

        [HttpGet]

        public async Task<ActionResult<User>> GetAllUsers() {
         
            return Ok(await _authContext.Users.ToListAsync()); 
        
        
        }






    }

}
