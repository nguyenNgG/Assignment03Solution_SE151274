using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Orders
{
    public class PrepareModel : PageModel
    {
        HttpSessionStorage sessionStorage;
        public PrepareModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public string Message { get; set; }
        public string HasItems { get; set; }
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
                        Message = (Cart.CartDetails.Count <= 0) ? "No items have been added to order." : "";
                        HasItems = (Cart.CartDetails.Count <= 0) ? "disabled" : "";
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
