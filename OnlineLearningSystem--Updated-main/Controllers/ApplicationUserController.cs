using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using OnlineLearningSystem.Models;
using System.Text;
using OnlineLearningSystem.Services;
using OnlineLearningSystem.Dtos;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using NETCore.MailKit.Core;
using System.IO;

namespace OnlineLearningSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _singInManager;
        private readonly ApplicationSettings _appSettings;
        private ICourseRepository _courseRepository;
        private IEmailService _emailService;

        public ApplicationUserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> appSettings,
            ICourseRepository courseRepository,
            IEmailService emailservice)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            _appSettings = appSettings.Value;
            _courseRepository = courseRepository;
            _emailService = emailservice;
        }

        [HttpPost]
        [Route("register")]
        //POST : /api/ApplicationUser/register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            //The role assigned when a new member signs up
            model.Role = "Student";
            var applicationUser = new ApplicationUser() {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, model.Role);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    //Build the confirmation link
                    var confirmationLink = Url.Action("ConfirmEmail", "ApplicationUser",
                        new { userId = applicationUser.Id, token = token }, Request.Scheme);
                    await _emailService.SendAsync(model.Email, "Email Verification",
                        $"<a href=\"{confirmationLink}\">Verify Email</a>", true);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        
        public async Task<String> ConfirmEmail(string userId, string token)
        {
            if (userId == null) 
                return "Something went wrong";

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return "Something went wrong";

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return "Your Email Has Been Confirmed";
            else
                return "Something went wrong!";
        }

        [HttpPost]
        [Route("mentor")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //POST : /api/ApplicationUser/mentor
        public async Task<Object> PostMentor(ApplicationUserModel model)
        {
            //The role assigned when a new member signs up
            model.Role = "Mentor";
            var applicationUser = new ApplicationUser() {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("login")]
        //POST : /api/ApplicationUser/login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if(user == null)
                return BadRequest(new { message = "Username or password is incorrect." });

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    //Get role assigned to the user
                    var role = await _userManager.GetRolesAsync(user);
                    IdentityOptions _options = new IdentityOptions();

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault()),
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok(new { token });
                }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
            }
            else
                return BadRequest(new { message = "Email is not confirmed"});

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("info")]
        //GET: /api/ApplicationUser/profile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.FullName,
                user.Email,
                user.UserName
            };
        }
    }
}