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
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return RedirectToPage(PageRoute.Home);
                }
                return Page();
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

                var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                var body = new StringContent(JsonSerializer.Serialize(Input), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Endpoints.Login, body);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return RedirectToPage(PageRoute.Home);
                    case HttpStatusCode.Forbidden:
                        ErrorMessage = "This account has been locked out, please try again later.";
                        return Page();
                    case HttpStatusCode.BadRequest:
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
