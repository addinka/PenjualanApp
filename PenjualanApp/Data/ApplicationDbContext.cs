﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PenjualanApp.Models;

namespace PenjualanApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet untuk tabel tambahan yang Anda miliki
        public DbSet<Produk> Produk { get; set; } = null!;
        public DbSet<Transaksi> Transaksi { get; set; } = null!;
        public DbSet<DetailTransaksi> DetailTransaksi { get; set; } = null!;

        // Metode untuk memanggil stored procedure
        public async Task<List<Transaksi>> GetLaporanTransaksiAsync()
        {
            return await this.Transaksi
                .FromSqlRaw("EXEC GetLaporanTransaksi") // Memanggil stored procedure
                .ToListAsync(); // Mengubah hasil query menjadi list
        }
    }
}
