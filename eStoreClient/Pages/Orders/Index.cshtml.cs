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

namespace eStoreClient.Pages.Orders
{
    public class IndexModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public IndexModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public List<Order> Orders { get; set; }
        public bool IsStaff { get; set; } = false;

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string memberId = await content.ReadAsStringAsync();
                    string query = $"?$filter=MemberId eq '{memberId}'&$expand=Member";

                    response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        IsStaff = true;
                        query = "?$expand=Member";
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Orders}{query}");
                    content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Orders = JsonSerializer.Deserialize<ODataModels<Order>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                        return Page();
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
