using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net;
using eStoreClient.Utilities;
using eStoreClient.Constants;
using BusinessObject;

namespace eStoreClient.Pages.Members
{
    public class DetailsModel : PageModel
    {
        HttpSessionStorage sessionStorage;

        public DetailsModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public Member Member { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return RedirectToPage(PageRoute.Home);
                }

                // check if is admin, if not, check if allowed to view
                var response = await SessionHelper.Current(HttpContext.Session, sessionStorage);
                var content = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? memberId = await content.ReadAsStringAsync();

                    response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        memberId = null;
                    }

                    if (!string.IsNullOrWhiteSpace(id) && memberId != id)
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
    }
}
