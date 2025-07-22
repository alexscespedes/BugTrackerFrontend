using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BugTrackerFrontend.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public LoginModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public LoginInput Input { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _clientFactory.CreateClient();
            var json = JsonSerializer.Serialize(Input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5020/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Invalid login attemp.";
                return Page();
            }

            var result = await response.Content.ReadAsStringAsync();
            var jwt = JsonDocument.Parse(result).RootElement.GetProperty("token").GetString();

            TempData["JwtToken"] = jwt;
            return RedirectToPage("/Bugs/Index");
        }

        public class LoginInput
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
    
}


