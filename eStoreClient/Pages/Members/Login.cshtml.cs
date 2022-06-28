using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Members
{
    public class LoginModel : PageModel
    {
        private readonly HttpSessionStorage sessionStorage;
        public LoginModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [BindProperty]
        public LoginInput Input { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                // add login check
            }
            catch
            {
            }
            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                HttpClient httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                StringContent body = new StringContent(JsonSerializer.Serialize(Input), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(Endpoints.Login, body);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToPage(PageRoute.Home);
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    ErrorMessage = "This account has been locked out, please try again later.";
                    return Page();
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ErrorMessage = "Email/password entered was invalid.";
                    return Page();
                }
            }
            catch
            {
            }
            return Page();
        }
    }
}
