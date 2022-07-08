using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Carts
{
    public class CreateModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public CreateModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public bool IsStaff { get; set; } = false;
        public string ProductMessage { get; set; }
        public string QuantityMessage { get; set; }

        [BindProperty]
        public ProductItem ProductItem { get; set; }

        [BindProperty]
        public CartDetail CartDetail { get; set; }

        public List<Product> Products { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        IsStaff = true;
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Products}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Products = JsonSerializer.Deserialize<ODataModels<Product>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                        response = await httpClient.GetAsync($"{Endpoints.Cart}");
                        content = response.Content;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Cart cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                            foreach (var detail in cart.CartDetails)
                            {
                                Products.RemoveAll(x => x.ProductId == detail.ProductItem.ProductId);
                            }
                        }
                        ViewData["ProductId"] = new SelectList(Products, "ProductId", "ProductName");
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

        public async Task<ActionResult> OnPostAsync()
        {
            try
            {
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        IsStaff = true;
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Products}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Products = JsonSerializer.Deserialize<ODataModels<Product>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                        response = await httpClient.GetAsync($"{Endpoints.Cart}");
                        content = response.Content;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Cart cart = JsonSerializer.Deserialize<Cart>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);

                            foreach (var detail in cart.CartDetails)
                            {
                                Products.RemoveAll(x => x.ProductId == detail.ProductItem.ProductId);
                            }

                            ViewData["ProductId"] = new SelectList(Products, "ProductId", "ProductName");

                            bool isExisted = cart.CartDetails.Where(cartDetail => cartDetail.ProductItem.ProductId == ProductItem.ProductId).Any();
                            if (isExisted)
                            {
                                ProductMessage = "Product already exists in order.";
                                return Page();
                            }

                            response = await httpClient.GetAsync($"{Endpoints.Products}/{ProductItem.ProductId}");
                            content = response.Content;
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                if (!ModelState.IsValid)
                                {
                                    return Page();
                                }

                                Product product = JsonSerializer.Deserialize<Product>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                                int? unitsInStock = product.UnitsInStock;
                                if (product.UnitsInStock < CartDetail.Quantity || CartDetail.Quantity <= 0)
                                {
                                    QuantityMessage = $"Only up to {unitsInStock} units can be added.";
                                    return Page();
                                }
                                ProductItem.ProductName = product.ProductName;
                                ProductItem.UnitPrice = product.UnitPrice;
                                CartDetail.ProductItem = ProductItem;
                                cart.CartDetails.Add(CartDetail);
                                StringContent body = new StringContent(JsonSerializer.Serialize(cart), Encoding.UTF8, "application/json");
                                response = await httpClient.PostAsync(Endpoints.Cart, body);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    return RedirectToPage(PageRoute.Cart);
                                }
                                return Page();
                            }
                        }
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
