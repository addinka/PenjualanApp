namespace PenjualanApp.Models
{
    public class DetailTransaksi
    {
        public int Id { get; set; }
        public int TransaksiId { get; set; }
        public int ProdukId { get; set; }
        public int Jumlah { get; set; }
        public decimal Subtotal { get; set; }
    }
}
