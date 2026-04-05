using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInvoice.MVC.Models;
using SmartInvoice.MVC.Services;
using System.Security.Claims;

namespace SmartInvoice.MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly ClientService _clientService;

        public PaymentController(PaymentService paymentService, ClientService clientService)
        {
            _paymentService = paymentService;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetClients();
            ViewBag.Clients = clients;

            // Get payments for logged-in user
            var payments = await _paymentService.GetPaymentsByUser();

            return View(payments);
        }




        [HttpPost]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid form data";
                return RedirectToAction("Index");
            }

            var result = await _paymentService.CreatePayment(model);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Payment recorded successfully!";
            return RedirectToAction("Index");
        }
    }
}