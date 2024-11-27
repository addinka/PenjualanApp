using Microsoft.AspNetCore.Mvc;
using PenjualanApp.Data;
using PenjualanApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace PenjualanApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Menampilkan halaman Manage Users
        public IActionResult ManageUsers()
        {
            var users = _context.Users.ToList(); // Mengambil daftar pengguna dari database
            return View(users);
        }
        // Untuk mengedit pengguna
        public IActionResult Edit(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Untuk menghapus pengguna
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageUsers));
        }
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
