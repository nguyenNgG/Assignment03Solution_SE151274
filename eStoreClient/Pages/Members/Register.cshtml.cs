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
    public class RegisterModel : PageModel
    {
        private readonly HttpSessionStorage sessionStorage;

        public RegisterModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [BindProperty]
        public RegisterInput Input { get; set; }

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
                var response = await httpClient.PostAsync(Endpoints.Register, body);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return RedirectToPage(PageRoute.Home);
                }
            }
            catch
            {
            }
            return Page();
        }
    }
}
