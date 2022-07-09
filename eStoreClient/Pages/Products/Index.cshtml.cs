using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Products
{
    public class IndexModel : PageModel
    {
        HttpSessionStorage sessionStorage;
        public IndexModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public IList<Product> Products { get; set; }

        [BindProperty]
        public string Filter { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Products}?$expand=Category");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Products = JsonSerializer.Deserialize<ODataModels<Product>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                    }
                    return Page();
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Home);
        }
    }
}
