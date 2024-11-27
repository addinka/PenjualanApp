namespace PenjualanApp.Models
{
    public class Transaksi
    {
        public int Id { get; set; }
        public string KasirId { get; set; } = string.Empty; // Default value
        public decimal Total { get; set; }
        public DateTime Tanggal { get; set; }
    }
}
