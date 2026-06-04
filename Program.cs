using Microsoft.EntityFrameworkCore;
using MultiTrack.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Session ekle
builder.Services.AddSession();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connString) && connString.StartsWith("postgres://"))
{
    var uri = new Uri(connString);
    var userInfo = uri.UserInfo.Split(':');
    connString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SslMode=Require;Trust Server Certificate=True;";
}

builder.Services.AddDbContext<MultiTrackDbContext>(options =>
    options.UseNpgsql(connString)
);

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
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// 🔥 OTOMATİK VERİTABANI KURULUMU (Canlı sunucu için)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MultiTrackDbContext>();
    db.Database.Migrate();
}

app.Run();