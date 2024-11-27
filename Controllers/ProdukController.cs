using Microsoft.AspNetCore.Mvc;
using PenjualanApp.Data;
using PenjualanApp.Models;

namespace PenjualanApp.Controllers
{
    public class ProdukController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdukController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var produk = _context.Produk.ToList();
            return View(produk);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Produk produk)
        {
            if (ModelState.IsValid)
            {
                _context.Produk.Add(produk);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produk);
        }
    }
}
