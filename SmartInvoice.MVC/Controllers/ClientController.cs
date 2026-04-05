using Microsoft.AspNetCore.Mvc;
using SmartInvoice.MVC.Models;
using SmartInvoice.MVC.Services;

namespace SmartInvoice.MVC.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetClients();
            var invoices = await _clientService.GetInvoices();

            ViewBag.Invoices = invoices;

            return View(clients);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid) return View(client);

            var success = await _clientService.CreateClient(client);
            if (!success) TempData["Error"] = "Failed to add client.";

            TempData["Success"] = "Client added successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}