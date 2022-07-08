using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Products
{
    public class DetailsModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public DetailsModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToPage(PageRoute.Products);
                }

                var response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Products}/{id}?$expand=Category");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Product = JsonSerializer.Deserialize<Product>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                        return Page();
                    }
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Products);
        }
    }
}
