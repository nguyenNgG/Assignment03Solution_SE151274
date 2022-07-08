using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Carts
{
    public class DetailsModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public DetailsModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public Cart Cart { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Cart}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                        return Page();
                    }
                }
                return RedirectToPage(PageRoute.Orders);
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Home);
        }
    }
}
