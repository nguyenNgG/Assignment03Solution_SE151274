using BusinessObject;
using DataAccess.Repositories.Interfaces;
using eStoreAPI.Constants;
using eStoreAPI.Models;
using eStoreClient.Constants;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
        private readonly IMemberRepository repository;

        public MembersController(SignInManager<Member> _signInManager,
            UserManager<Member> _userManager, IConfiguration _configuration, IMemberRepository _repository)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
            repository = _repository;
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
        [Authorize]
        public ActionResult Current()
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    return Ok(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("authorize")]
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Authorize()
        {
            try
            {
                if (User.IsInRole(RoleName.Administrator))
                {
                    return Ok(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("cart")]
        [Authorize]
        public ActionResult<Cart> GetCart()
        {
            Cart cart = SessionHelper.GetFromSession<Cart>(HttpContext.Session, SessionValue.Cart);
            if (cart == null)
            {
                cart = new Cart();
            }
            return Ok(cart);
        }

        [HttpPost("cart")]
        [Authorize]
        public ActionResult<Cart> SetCart(Cart cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }
            SessionHelper.SaveToSession<Cart>(HttpContext.Session, cart, SessionValue.Cart);
            return Ok(cart);
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Member>>> Get()
        {
            var list = await repository.GetList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        [Authorize]
        public async Task<ActionResult<Member>> GetMember([FromODataUri] string key)
        {
            var obj = await repository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
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
