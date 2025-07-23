using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BugTrackerFrontend.Pages.Bugs
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public BugDto Bug { get; set; } = new();

        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BugApi");
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var content = new StringContent(JsonSerializer.Serialize(Bug), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/bugs", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Error creating bug.");
            return Page();
        }
    }
}
