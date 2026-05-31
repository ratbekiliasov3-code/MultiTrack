using Microsoft.EntityFrameworkCore;
using MultiTrack.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- HAFIZA VERİTABANI (ŞİFRESİZ MOD) ---
// MySQL bağlantı kodları tamamen kaldırıldı.
builder.Services.AddDbContext<MultiTrackDbContext>(options =>
    options.UseInMemoryDatabase("MultiTrackTestDb"));
// ----------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();