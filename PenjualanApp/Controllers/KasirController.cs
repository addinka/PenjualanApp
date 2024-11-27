using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PenjualanApp.Data; // Pastikan namespace sesuai
using System.Linq; // Untuk LINQ

[Authorize(Roles = "Kasir")]
public class KasirController : Controller
{
    private readonly ApplicationDbContext _context;

    public KasirController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Transaksi()
    {
        // Ambil data transaksi dari database
        var transaksiList = _context.Transaksi.ToList();

        // Kirim data ke view
        return View(transaksiList);
    }

    public IActionResult Laporan()
    {
        return View();
    }
}
