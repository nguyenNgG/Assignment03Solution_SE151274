using BusinessObject;
using eStoreAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Threading.Tasks;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ODataController
    {
        private readonly UserManager<Member> userManager;
        private readonly SignInManager<Member> signInManager;

        public MembersController(SignInManager<Member> _signInManager,
            UserManager<Member> _userManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
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
                    await signInManager.SignInAsync(member, isPersistent: false);
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
                var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return Ok();
                }
                if (result.IsLockedOut)
                {
                    return Forbid();
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
