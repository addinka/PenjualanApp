namespace PenjualanApp.Models
{
    public class LoginModel
    {
        // Properti untuk menangani input username
        public string Username { get; set; }

        // Properti untuk menangani input password
        public string Password { get; set; }

        // Properti untuk menangani checkbox "Ingat Saya" (Remember Me)
        public bool RememberMe { get; set; }
    }
}
