using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PenjualanApp.Data;
using QuestPDF.Infrastructure;
QuestPDF.Settings.License = LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);

// Konfigurasi DbContext untuk Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfigurasi Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // Tambahkan dukungan role
.AddEntityFrameworkStores<ApplicationDbContext>();

// Tambahkan controller dengan view
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Scope untuk konfigurasi role dan user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Tambahkan role Admin jika belum ada
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Tambahkan role Kasir jika belum ada
    if (!await roleManager.RoleExistsAsync("Kasir"))
    {
        await roleManager.CreateAsync(new IdentityRole("Kasir"));
    }

    // Tambahkan Admin default jika belum ada
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "Admin@123"); // Password default
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Tambahkan middleware Authentication dan Authorization
app.UseAuthentication();
app.UseAuthorization();

// Middleware untuk redirect ke halaman login jika belum login
app.Use(async (context, next) =>
{
    // Redirect ke halaman login jika user belum login dan bukan di area /Identity/Account
    if (!context.User.Identity.IsAuthenticated &&
        !context.Request.Path.StartsWithSegments("/Identity/Account"))
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }
    await next();
});

// Konfigurasi route default
app.MapControllerRoute(
    name: "default",
     pattern: "{controller=Transaksi}/{action=Index}/{id?}");

app.MapRazorPages(); // Untuk mendukung area Identity

app.Run();
