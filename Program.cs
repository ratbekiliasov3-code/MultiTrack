using Microsoft.EntityFrameworkCore;
using MultiTrack.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Session ekle
builder.Services.AddSession();

builder.Services.AddDbContext<MultiTrackDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Middleware sırası ÇOK önemli
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 BURAYA EKLE
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();