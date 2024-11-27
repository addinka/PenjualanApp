using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using PenjualanApp.Data;
using QuestPDF.Helpers;

namespace PenjualanApp.Controllers
{
    public class LaporanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LaporanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "Selamat datang di halaman laporan.";
            return View();
        }

        public IActionResult ExportToPDF()
        {
            var transaksiList = _context.Transaksi.ToList(); // Ambil data dari database

            // Buat PDF menggunakan QuestPDF
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Header()
                        .Text("Laporan Transaksi")
                        .SemiBold().FontSize(18).AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            // Header
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1); // ID
                                columns.RelativeColumn(2); // Kasir
                                columns.RelativeColumn(2); // Total
                                columns.RelativeColumn(3); // Tanggal
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID");
                                header.Cell().Text("Kasir");
                                header.Cell().Text("Total");
                                header.Cell().Text("Tanggal");
                            });

                            // Isi tabel
                            foreach (var transaksi in transaksiList)
                            {
                                table.Cell().Text(transaksi.Id.ToString());
                                table.Cell().Text(transaksi.KasirId);
                                table.Cell().Text(transaksi.Total.ToString("C"));
                                table.Cell().Text(transaksi.Tanggal.ToString("yyyy-MM-dd"));
                            }
                        });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();

            return File(pdfBytes, "application/pdf", "LaporanTransaksi.pdf");
        }
    }
}
