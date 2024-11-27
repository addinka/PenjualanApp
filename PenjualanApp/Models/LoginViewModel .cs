using System.ComponentModel.DataAnnotations;

namespace PenjualanApp.Models
{
    public class LoginViewModel
    {
        // Properti untuk Username yang wajib diisi
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        // Properti untuk Password yang wajib diisi
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)] // Menggunakan jenis data password
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Properti untuk RememberMe (Ingat Saya)
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
