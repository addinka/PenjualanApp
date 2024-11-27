using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using OfficeOpenXml;  // Untuk Excel Export
using PenjualanApp.Data;
using PenjualanApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
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

        // Tampilkan laporan ke view menggunakan stored procedure
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Laporan";
            ViewBag.Message = "Selamat datang di halaman laporan.";
            var laporanData = await _context.GetLaporanTransaksiAsync(); // Ambil data dari stored procedure
            return View(laporanData); // Kirim data ke view
        }

        // Export laporan ke PDF menggunakan QuestPDF
        public async Task<IActionResult> ExportToPDF()
        {
            var transaksiList = await _context.GetLaporanTransaksiAsync(); // Ambil data dari stored procedure

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
                            // Header kolom tabel
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1); // ID
                                columns.RelativeColumn(2); // Kasir
                                columns.RelativeColumn(2); // Total
                                columns.RelativeColumn(3); // Tanggal
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Kasir").Bold();
                                header.Cell().Text("Total").Bold();
                                header.Cell().Text("Tanggal").Bold();
                            });

                            // Isi tabel
                            foreach (var transaksi in transaksiList)
                            {
                                table.Cell().Text(transaksi.Id.ToString());
                                table.Cell().Text(transaksi.KasirId);
                                table.Cell().Text(transaksi.Total.ToString("C")); // Format mata uang
                                table.Cell().Text(transaksi.Tanggal.ToString("yyyy-MM-dd"));
                            }
                        });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();

            // Return file PDF
            return File(pdfBytes, "application/pdf", "LaporanTransaksi.pdf");
        }

        // Export laporan ke Excel menggunakan EPPlus
        public async Task<IActionResult> ExportToExcel()
        {
            var transaksiList = await _context.GetLaporanTransaksiAsync(); // Ambil data dari stored procedure

            if (transaksiList == null || !transaksiList.Any())
            {
                TempData["ErrorMessage"] = "Tidak ada data transaksi untuk diekspor.";
                return RedirectToAction("Index");
            }

            // Menggunakan EPPlus untuk membuat Excel
            using (var package = new ExcelPackage())
            {
                // Buat worksheet baru
                var worksheet = package.Workbook.Worksheets.Add("Laporan Transaksi");

                // Menambahkan header di baris pertama
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Kasir ID";
                worksheet.Cells[1, 3].Value = "Total";
                worksheet.Cells[1, 4].Value = "Tanggal";

                // Set header menjadi bold
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                }

                // Menambahkan data ke dalam worksheet
                var row = 2;
                foreach (var transaksi in transaksiList)
                {
                    worksheet.Cells[row, 1].Value = transaksi.Id;
                    worksheet.Cells[row, 2].Value = transaksi.KasirId;
                    worksheet.Cells[row, 3].Value = transaksi.Total;
                    worksheet.Cells[row, 4].Value = transaksi.Tanggal.ToString("yyyy-MM-dd HH:mm:ss");
                    row++;
                }

                // Format kolom Total dengan format angka
                worksheet.Column(3).Style.Numberformat.Format = "#,##0.00";

                // Mengatur lebar kolom agar otomatis menyesuaikan konten
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Menghasilkan file Excel dalam format stream
                var fileBytes = package.GetAsByteArray();

                // Mengembalikan file Excel untuk diunduh
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LaporanTransaksi.xlsx");
            }
        }
    }
}
