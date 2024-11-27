namespace PenjualanApp.Models
{
    public class Produk
    {
        public int Id { get; set; }
        public string Nama { get; set; } = string.Empty; // Default value
        public decimal Harga { get; set; }
        public int Stok { get; set; }
    }
}
