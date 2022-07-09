using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Orders
{
    public class EditModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public EditModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public string Message { get; set; }

        [FromQuery(Name = "order-id")]
        public string OrderId { get; set; }

        [BindProperty]
        public EditOrderForm EditOrderForm { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OrderId))
                {
                    return RedirectToPage(PageRoute.Orders);
                }

                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Orders}/{OrderId}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Order order = JsonSerializer.Deserialize<Order>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);

                        EditOrderForm = new()
                        {
                            Freight = order.Freight,
                            ShippedDate = order.ShippedDate,
                        };
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

        public async Task<ActionResult> OnPostAsync(string orderId)
        {
            try
            {
                if (EditOrderForm == null)
                {
                    return RedirectToPage(PageRoute.Orders);
                }

                var response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Orders}/{OrderId}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Order order = JsonSerializer.Deserialize<Order>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);

                        if (!ModelState.IsValid)
                        {
                            return Page();
                        }

                        if (EditOrderForm.ShippedDate != null)
                        {
                            if (((DateTime)EditOrderForm.ShippedDate).CompareTo(order.OrderDate) <= 0)
                            {
                                Message = $"Shipped Date must be after Order Date: {order.OrderDate}";
                                return Page();
                            }
                            else
                            {
                                Message = "";
                            }
                        }
                        order.Freight = EditOrderForm.Freight;
                        order.ShippedDate = EditOrderForm.ShippedDate.Value.ToUniversalTime();
                        string v = JsonSerializer.Serialize(order);
                        StringContent body = new StringContent(v, Encoding.UTF8, "application/json");
                        response = await httpClient.PutAsync($"{Endpoints.Orders}/{OrderId}", body);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return RedirectToPage(PageRoute.Orders);
                        }
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
