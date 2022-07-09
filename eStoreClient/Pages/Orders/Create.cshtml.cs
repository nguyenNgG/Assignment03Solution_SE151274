using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Orders
{
    public class CreateModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public CreateModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [BindProperty]
        public Order Order { get; set; }
        public Cart Cart { get; set; }
        public List<Member> Members { get; set; }

        public string IdTakenMessage { get; set; }

        public string? MemberId { get; set; } = null!;

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                // check if is admin, if not, hide member selection, use memberId from current as value
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MemberId = await content.ReadAsStringAsync();

                    response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        MemberId = null;
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Members}");
                    content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Members = JsonSerializer.Deserialize<ODataModels<Member>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;

                        Members.RemoveAll(x => x.Email == "admin@estore.com");

                        if (MemberId != null)
                        {
                            Members.RemoveAll(x => x.Id != MemberId);
                        }

                        ViewData["MemberId"] = new SelectList(Members, "Id", "Email");

                        response = await httpClient.GetAsync($"{Endpoints.Cart}");
                        content = response.Content;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                            if (Cart.CartDetails.Count <= 0)
                            {
                                return RedirectToPage(PageRoute.OrderPrepare);
                            }
                            return Page();
                        }
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
            return RedirectToPage(PageRoute.Home);
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                IdTakenMessage = "";
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MemberId = await content.ReadAsStringAsync();

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Members}");
                    content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Members = JsonSerializer.Deserialize<ODataModels<Member>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;

                        if (MemberId != null)
                        {
                            Members.RemoveAll(x => x.Id != MemberId);
                        }

                        ViewData["MemberId"] = new SelectList(Members, "Id", "Email");

                        response = await httpClient.GetAsync($"{Endpoints.Cart}");
                        content = response.Content;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                            if (Cart.CartDetails.Count <= 0)
                            {
                                return RedirectToPage(PageRoute.OrderPrepare);
                            }

                            if (!ModelState.IsValid)
                            {
                                return Page();
                            }

                            Order.RequiredDate = Order.RequiredDate.Value.ToUniversalTime();
                            Order.OrderDate = DateTime.Now.ToUniversalTime();
                            foreach (var detail in Cart.CartDetails)
                            {
                                OrderDetail orderDetail = new()
                                {
                                    ProductId = detail.ProductItem.ProductId,
                                    Quantity = detail.Quantity,
                                    UnitPrice = (decimal)detail.ProductItem.UnitPrice,
                                    Discount = detail.Discount,
                                };
                                Order.OrderDetails.Add(orderDetail);

                                response = await httpClient.GetAsync($"{Endpoints.Products}/{orderDetail.ProductId}");
                                content = response.Content;
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    Product product = JsonSerializer.Deserialize<Product>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                                    product.UnitsInStock -= (int)orderDetail.Quantity!;
                                    if (product.UnitsInStock < 0) product.UnitsInStock = 0;
                                    StringContent body = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
                                    response = await httpClient.PutAsync($"{Endpoints.Products}/{product.ProductId}", body);
                                }
                                if (response.StatusCode == HttpStatusCode.NotFound)
                                {
                                    return RedirectToPage(PageRoute.Orders);
                                }
                            }
                            string ord = JsonSerializer.Serialize(Order);
                            StringContent orderBody = new StringContent(ord, Encoding.UTF8, "application/json");
                            response = await httpClient.PostAsync($"{Endpoints.Orders}", orderBody);

                            if (response.StatusCode == HttpStatusCode.Conflict)
                            {
                                IdTakenMessage = Message.IdTaken;
                                return Page();
                            }

                            Cart.CartDetails.Clear();
                            StringContent cartBody = new StringContent(JsonSerializer.Serialize(Cart), Encoding.UTF8, "application/json");
                            response = await httpClient.PostAsync(Endpoints.Cart, cartBody);

                            return RedirectToPage(PageRoute.Orders);
                        }
                    }
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
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
