using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace aspnet_mvc_tempdata_enum.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [TempData]
        public string Message { get; set; }

        [TempData]
        public StateEnum State { get; set; }

        [TempData]
        public int StateInt { get; set; }

        public string RequestCookieData { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Check cookie values.
            // REF: https://github.com/dotnet/aspnetcore/blob/v3.1.23/src/Mvc/Mvc.ViewFeatures/src/CookieTempDataProvider.cs#L63
            var encodedValue = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            if (encodedValue != null)
            {
                var protectedData = WebEncoders.Base64UrlDecode(encodedValue);
                var dataProtector = this.HttpContext.RequestServices.GetDataProtector("Microsoft.AspNetCore.Mvc.CookieTempDataProviderToken.v1");
                var unprotectedData = dataProtector.Unprotect(protectedData);

                RequestCookieData = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(unprotectedData));
            }

            return Page();
        }

        public IActionResult OnGetOKHandler()
        {
            State = StateEnum.OK;
            StateInt = (int)StateEnum.OK;
            Message = "OK!!!!";

            return Redirect(Request.Path);
        }

        public IActionResult OnGetNGHandler()
        {
            State = StateEnum.NG;
            StateInt = (int)StateEnum.NG;
            Message = "NG...";

            return Redirect(Request.Path);
        }
    }

    public enum StateEnum
    {
        VIEW,
        OK,
        NG
    }
}
