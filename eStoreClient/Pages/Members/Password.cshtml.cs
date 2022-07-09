using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Models;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Members
{
    public class PasswordModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public PasswordModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        [BindProperty]
        public NewPasswordInput Input { get; set; }
        public Member Member { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return RedirectToPage(PageRoute.Home);
                }

                // check if allowed to edit
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? memberId = await content.ReadAsStringAsync();

                    if (memberId != id)
                    {
                        return RedirectToPage(PageRoute.Home);
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    response = await httpClient.GetAsync($"{Endpoints.Members}('{id}')");
                    content = response.Content;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Member = JsonSerializer.Deserialize<Member>(await content.ReadAsStringAsync(), SerializerOptions.CaseInsensitive);
                        return Page();
                    }
                }
            }
            catch
            {
            }
            return RedirectToPage(PageRoute.Home);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? memberId = await content.ReadAsStringAsync();

                    if (!ModelState.IsValid)
                    {
                        return Page();
                    }

                    var httpClient = SessionHelper.GetHttpClient(HttpContext.Session, sessionStorage);
                    var body = new StringContent(JsonSerializer.Serialize(Input), Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync($"{Endpoints.Password}", body);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToPage(PageRoute.Profile, new { id = memberId });
                    }
                    else
                    {
                        ErrorMessage = "The input was invalid.";
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
