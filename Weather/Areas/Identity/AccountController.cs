using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weather.Areas.Identity.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _config;

#if DEBUG
        private string userName;
        private string userPassword;
        private string secureApi;
#endif

        public AccountController(IConfiguration config, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger)
        {
            _logger = logger;
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;

            userName = _config["UserName"];
            userPassword = _config["UserPassword"];
            secureApi = _config["SecureApi"];
        }

#if DEBUG
        public AccountController()
        {
        }
#endif

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLogin()
        {
            var user = await _userManager.FindByNameAsync(userName);
#if DEBUG
            if (await _userManager.FindByNameAsync(_config["UserName"]) == null)
            {

            }
#endif
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var Claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, userName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var authSigniningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureApi));

                    var token = new JwtSecurityToken(
                        expires: DateTime.Now.AddHours(3),
                        claims: Claims,
                        signingCredentials: new SigningCredentials(authSigniningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        message = "User logged in",
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return Unauthorized();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest("Login failed. User not logged in");
            }
        }
#if DEBUG
        public async Task<IdentityUser> CreateUser(string email, string name, string defaultPassword, UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    Email = email,
                    UserName = name,
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(newUser, defaultPassword);

                if (result.Succeeded)
                {
                    return newUser;
                }
            }

            return user;
        }
#endif
    }
}
