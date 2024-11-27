using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PenjualanApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // Login action
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            // Mencoba untuk login
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            // Jika login berhasil
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var roles = await _userManager.GetRolesAsync(user);

                // Cek jika user memiliki role Admin
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin"); // Redirect ke Admin Dashboard
                }
                // Cek jika user memiliki role Kasir
                else if (roles.Contains("Kasir"))
                {
                    return RedirectToAction("Dashboard", "Kasir"); // Redirect ke Kasir Dashboard
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Redirect ke halaman default
                }
            }
            else
            {
                // Jika login gagal
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(model); // Menampilkan kembali form login jika gagal
    }
}
