using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace SmartInvoice.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("API");

            var data = new
            {
                email = email,
                password = password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login";
                return View();
            }

            // Read the JWT token from API response
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);
            var token = json.RootElement.GetProperty("token").GetString();

            // Store token in session for future API calls
            HttpContext.Session.SetString("JWToken", token);

            return RedirectToAction("Index", "Home"); // redirect to clients page
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var client = _httpClientFactory.CreateClient("API");

            var data = new
            {
                name = name,
                email = email,
                password = password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("api/auth/register", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Registration failed";
                return View();
            }

            // After registration → go to login page
            return RedirectToAction("Login");
        }
    }
}