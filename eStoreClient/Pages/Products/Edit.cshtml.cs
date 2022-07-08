using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Products
{
    public class EditModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public EditModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [BindProperty]
        public Product Product { get; set; }

        [TempData]
        public int ProductId { get; set; }

        public List<Category> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToPage(PageRoute.Products);
                }

                ProductId = (int)id;
                TempData["ProductId"] = ProductId;

                var response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // get product
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Products}/{id}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Product = JsonSerializer.Deserialize<Product>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);

                        // get categories
                        response = await httpClient.GetAsync($"{Endpoints.Categories}");
                        content = response.Content;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Categories = JsonSerializer.Deserialize<ODataModels<Category>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                            ViewData["CategoryId"] = new SelectList(Categories, "CategoryId", "CategoryName");
                            return Page();
                        }
                    }
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Products);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Product.ProductId = (int)TempData.Peek("ProductId");
                TempData.Keep("ProductId");

                var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                var response = await httpClient.GetAsync($"{Endpoints.Categories}");
                var content = response.Content;

                Product = StringTrimmer.TrimProduct(Product);

                // get categories
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Categories = JsonSerializer.Deserialize<ODataModels<Category>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                    ViewData["CategoryId"] = new SelectList(Categories, "CategoryId", "CategoryName");
                }
                else
                {
                    return RedirectToPage(PageRoute.Products);
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                Product.Category = null;
                var body = new StringContent(JsonSerializer.Serialize(Product), Encoding.UTF8, "application/json");
                response = await httpClient.PutAsync($"{Endpoints.Products}/{ProductId}", body);
                content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Product = JsonSerializer.Deserialize<Product>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                    return RedirectToPage(PageRoute.Products);
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Products);
        }
    }
}
