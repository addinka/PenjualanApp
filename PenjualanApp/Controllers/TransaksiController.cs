using Microsoft.AspNetCore.Mvc;
using PenjualanApp.Data;
using PenjualanApp.Models;
using ServiceReference1; // Namespace dari service reference

namespace PenjualanApp.Controllers
{
    public class TransaksiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransaksiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult FormTransaksi()
        {
            ViewData["ActiveMenu"] = "Transaksi";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string ProductName, int Quantity, decimal Price)
        {
            decimal total = 0;

            // Gunakan web service untuk menghitung total
            try
            {
                CalculatorSoapClient client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                int response = await client.MultiplyAsync(Quantity, (int)Price); // Hitung total = Quantity * Price
                total = response; // Gunakan langsung response sebagai total
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Gagal menghitung total menggunakan web service. " + ex.Message;
                return RedirectToAction("FormTransaksi");
            }

           

            // Simpan data transaksi ke database
            var transaksi = new Transaksi
            {
                KasirId = User.Identity.Name ?? "Unknown",
                Total = total,
                Tanggal = DateTime.Now
            };

            _context.Transaksi.Add(transaksi);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Transaksi berhasil disimpan dengan total: " + total;
            return RedirectToAction("DaftarTransaksi");
        }

        public IActionResult DaftarTransaksi()
        {
            ViewData["ActiveMenu"] = "ListTransaksi";
            var transaksiList = _context.Transaksi.ToList();
            return View(transaksiList);
        }

    }
}
