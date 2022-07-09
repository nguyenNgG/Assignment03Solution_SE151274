using BusinessObject;
using eStoreClient.Constants;
using eStoreClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStoreClient.Pages.Reports
{
    public class ReportModel : PageModel
    {
        HttpSessionStorage sessionStorage;
        public ReportModel(HttpSessionStorage _sessionStorage)
        {
            sessionStorage = _sessionStorage;
        }

        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                var response = await SessionHelper.Authorize(HttpContext.Session, sessionStorage);
                if (response.StatusCode == HttpStatusCode.OK)
                {
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
