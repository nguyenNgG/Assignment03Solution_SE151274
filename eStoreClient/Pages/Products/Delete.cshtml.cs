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
    public class DeleteModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public DeleteModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [BindProperty]
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
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToPage(PageRoute.Products);
                    }
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Products);
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToPage(PageRoute.Products);
                }

                var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                var response = await httpClient.DeleteAsync($"{Endpoints.Products}/{id}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToPage(PageRoute.Products);
                }
            }
            catch
            {
            }
            return RedirectToAction(nameof(OnGetAsync), new { id = id });
        }
    }
}
