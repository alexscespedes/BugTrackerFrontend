using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BugTrackerFrontend.Pages.Bugs
{
    public class BugsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public BugsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public List<BugDto> Bugs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = TempData["JwtToken"]?.ToString();

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login");
            }

            TempData.Keep("JwtToken");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("http:localhost:5020/api/bugs");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Bugs = JsonSerializer.Deserialize<List<BugDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }

            return Page();
        }
    }

    public class BugDto
    {
        public string Title { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
