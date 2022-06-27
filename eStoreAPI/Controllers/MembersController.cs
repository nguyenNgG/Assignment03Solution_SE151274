using BusinessObject;
using Microsoft.AspNetCore.Http;
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
