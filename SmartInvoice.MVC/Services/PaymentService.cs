using Microsoft.AspNetCore.Http;
using SmartInvoice.MVC.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SmartInvoice.MVC.Services
{
    public class PaymentService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public PaymentService(IHttpClientFactory httpFactory, IHttpContextAccessor context)
        {
            _http = httpFactory.CreateClient("API");
            _context = context;
        }

        private void AddToken()
        {
            var token = _context.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<PaymentListViewModel>> GetPaymentsByUser()
        {
            AddToken();

            // Call the new endpoint
            return await _http.GetFromJsonAsync<List<PaymentListViewModel>>("api/payments");
        }

        public async Task<(bool Success, string Message)> CreatePayment(PaymentViewModel payment)
        {
            try
            {
                AddToken();

                var response = await _http.PostAsJsonAsync("api/payments", new
                {
                    invoiceId = payment.InvoiceId,
                    amount = payment.Amount
                });

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return (true, "Success");

                return (false, content);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}