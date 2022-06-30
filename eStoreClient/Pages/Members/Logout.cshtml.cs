using eStoreClient.Constants;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Members
{
    public class LogoutModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public LogoutModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public async Task<ActionResult> OnGet()
        {
            try
            {
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Page();
                }
                return RedirectToPage(PageRoute.Login);
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Home);
        }

        public async Task<ActionResult> OnPost()
        {
            var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
            var body = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Endpoints.Logout, body);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SessionHelper.GetNewHttpClient(HttpContext.Session, sessionStorage);
                    return RedirectToPage(PageRoute.Login);
            }
            return Page();
        }
    }
}
