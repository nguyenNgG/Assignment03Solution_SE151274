using eStoreClient.Constants;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
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
            HttpResponseMessage response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Page();
            }
            return RedirectToPage(PageRoute.Login);
        }

        public ActionResult OnPost()
        {
            SessionHelper.GetNewHttpClient(HttpContext.Session, sessionStorage);
            HttpClient httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);

            return RedirectToPage(PageRoute.Login);
        }
    }
}
