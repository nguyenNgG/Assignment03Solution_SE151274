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
    public class CreateModel : PageModel
    {
        HttpSessionStorage sessionStorage;
        public CreateModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public List<Category> Categories { get; set; }

        [BindProperty]
        public Product Product { get; set; }
        public string IdTakenMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Categories}");
                    var content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Categories = JsonSerializer.Deserialize<ODataModels<Category>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                        ViewData["CategoryId"] = new SelectList(Categories, "CategoryId", "CategoryName");
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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                IdTakenMessage = "";
                Product = StringTrimmer.TrimProduct(Product);

                var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                var response = await httpClient.GetAsync($"{Endpoints.Categories}");
                var content = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Categories = JsonSerializer.Deserialize<ODataModels<Category>>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive).List;
                    ViewData["CategoryId"] = new SelectList(Categories, "CategoryId", "CategoryName");
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return RedirectToPage(PageRoute.Products);
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                var body = new StringContent(JsonSerializer.Serialize(Product), Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync(Endpoints.Products, body);
                content = response.Content;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Product = JsonSerializer.Deserialize<Product>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                    return RedirectToPage(PageRoute.Products);
                }
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    IdTakenMessage = Message.IdTaken;
                    return await OnGetAsync();
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Products);
        }
    }
}
