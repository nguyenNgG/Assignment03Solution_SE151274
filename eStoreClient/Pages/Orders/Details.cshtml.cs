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
    public class DetailsModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public DetailsModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [FromQuery(Name = "order-id")]
        public string OrderId { get; set; }
        public bool IsStaff { get; set; } = false;

        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OrderId))
                {
                    return RedirectToPage(PageRoute.Orders);
                }

                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        IsStaff = true;
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Orders}/{OrderId}");
                    content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Order = JsonSerializer.Deserialize<Order>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);

                        httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                        response = await httpClient.GetAsync($"{Endpoints.OrderDetails}?$filter=OrderId eq {Order.OrderId}");
                        content = response.Content;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            OrderDetails = JsonSerializer.Deserialize<ODataModels<OrderDetail>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                            return Page();
                        }
                        return RedirectToPage(PageRoute.Orders);
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
