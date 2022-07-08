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

namespace eStoreClient.Pages.Carts
{
    public class DeleteModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public DeleteModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [FromQuery(Name = "item-index")] public int ItemIndex { get; set; } = -1;
        public CartDetail CartDetail { get; set; }

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
                        Cart cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                        int itemCount = cart.CartDetails.Count;
                        if (ItemIndex <= itemCount && ItemIndex >= 0)
                        {
                            CartDetail = cart.CartDetails[ItemIndex];
                            return Page();
                        }
                        return RedirectToPage(PageRoute.Cart);
                    }
                }
                return RedirectToPage(PageRoute.Orders);
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Home);
        }

        public async Task<ActionResult> OnPostAsync()
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
                        Cart cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                        int itemCount = cart.CartDetails.Count;
                        if (ItemIndex <= itemCount && ItemIndex >= 0)
                        {
                            cart.CartDetails.RemoveAt(ItemIndex);
                            StringContent body = new StringContent(JsonSerializer.Serialize(cart), Encoding.UTF8, "application/json");
                            response = await httpClient.PostAsync(Endpoints.Cart, body);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                return RedirectToPage(PageRoute.Cart);
                            }
                            return Page();
                        }
                        return RedirectToPage(PageRoute.Cart);
                    }
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Home);
        }
    }
}
