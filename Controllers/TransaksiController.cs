using Microsoft.AspNetCore.Mvc;
using PenjualanApp.Data;
using PenjualanApp.Models;

namespace PenjualanApp.Controllers
{
    public class TransaksiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransaksiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Menampilkan halaman transaksi
        public IActionResult Index()
        {
            return View();
        }

        // Menangani form submit
        [HttpPost]
        public IActionResult Submit(string ProductName, int Quantity, decimal Price)
        {
            // Hitung total transaksi
            decimal Total = Quantity * Price;

            // Simpan data transaksi ke database
            var transaksi = new Transaksi
            {
                KasirId = User.Identity.Name ?? "Unknown",
                Total = Total,
                Tanggal = DateTime.Now
            };

            _context.Transaksi.Add(transaksi);
            _context.SaveChanges();

            TempData["Message"] = "Transaksi berhasil disimpan.";

            // Redirect kembali ke halaman transaksi
            return RedirectToAction("Index");
        }
    }
}
