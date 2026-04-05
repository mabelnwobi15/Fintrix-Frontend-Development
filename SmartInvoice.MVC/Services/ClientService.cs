using System.Net.Http.Headers;
using System.Net.Http.Json;
using SmartInvoice.MVC.Models;

namespace SmartInvoice.MVC.Services
{
    public class ClientService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService(IHttpClientFactory httpFactory, IHttpContextAccessor httpContextAccessor)
        {
            _http = httpFactory.CreateClient("API");
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Client>> GetClients()
        {
            AddAuthHeader();
            return await _http.GetFromJsonAsync<List<Client>>("api/clients");
        }

        public async Task<bool> CreateClient(Client client)
        {
            AddAuthHeader();
            var response = await _http.PostAsJsonAsync("api/clients", new
            {
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone
            });
            return response.IsSuccessStatusCode;
        }

        public async Task<List<InvoiceViewModel>> GetInvoices()
        {
            AddAuthHeader();

            var result = await _http.GetFromJsonAsync<List<InvoiceViewModel>>("api/invoices");

            return result ?? new List<InvoiceViewModel>();
        }


    }
}