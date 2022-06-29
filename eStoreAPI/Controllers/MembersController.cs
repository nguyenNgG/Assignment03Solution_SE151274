using BusinessObject;
using eStoreAPI.Models;
using eStoreClient.Constants;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ODataController
    {
        private readonly UserManager<Member> userManager;
        private readonly SignInManager<Member> signInManager;
        private readonly IConfiguration configuration;

        public MembersController(SignInManager<Member> _signInManager,
            UserManager<Member> _userManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterInput obj)
        {
            try
            {
                var member = new Member { UserName = obj.Email, Email = obj.Email };
                var result = await userManager.CreateAsync(member, obj.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginInput obj)
        {
            try
            {
                string adminEmail = configuration.GetValue<string>("Admin:Email");
                string adminPassword = configuration.GetValue<string>("Admin:Password");

                bool isAdmin = (obj.Email == adminEmail && obj.Password == adminPassword);

                if (isAdmin)
                {
                    obj.Email = "admin@estore.com";
                    obj.Password = "QWEasd123!";
                }

                var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return Ok();
                }
                if (result.IsLockedOut)
                {
                    return Forbid();
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("current")]
        public ActionResult Current()
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    return Ok(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Member>> Post(Member obj)
        {
            try
            {
                await userManager.CreateAsync(obj);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
