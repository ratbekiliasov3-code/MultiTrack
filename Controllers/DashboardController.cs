using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTrack.Models; // Modelleri ve DbContext'i görmesi için bu satır ŞART!
using System;
using System.Linq;
using System.Collections.Generic;

namespace MultiTrack.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MultiTrackDbContext _context;

        public DashboardController(MultiTrackDbContext context)
        {
            _context = context;
        }

        // Controllers/DashboardController.cs içindeki ilgili kısmı bulun ve sadece burayı değiştirin:
        [HttpGet]
[Route("Dashboard/Index")]
[Route("Dashboard/Main")] // Giriş yaptıktan sonra buraya yönlendireceğiz
public IActionResult Index()
{
    var username = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Index", "Home");

    ViewBag.Username = username;
    ViewBag.Username = username;

    var today = DateTime.Today;
    var todayStart = today.Date;
    var todayEnd = todayStart.AddDays(1);
    ViewBag.TodayExpenseTotal = _context.Harcamalar
        .Where(h => h.KullaniciId == username && h.Tarih >= todayStart && h.Tarih < todayEnd)
        .Sum(h => (double?)h.Tutar) ?? 0.0;

    var thisMonthStart = new DateTime(today.Year, today.Month, 1);
    var nextMonthStart = thisMonthStart.AddMonths(1);
    ViewBag.MonthlyExpenseTotal = _context.Harcamalar
        .Where(h => h.KullaniciId == username && h.Tarih >= thisMonthStart && h.Tarih < nextMonthStart)
        .Sum(h => (double?)h.Tutar) ?? 0.0;

    return View(); // Bizi ana 6'lı panelin olduğu sayfaya götürecek
}

[HttpGet]
[Route("Dashboard")]
[Route("Dashboard/GunlukPlan")]
public IActionResult GunlukPlan(string user, string? tarih)
{
    var username = HttpContext.Session.GetString("UserId");

if (string.IsNullOrEmpty(username))
    return RedirectToAction("Index", "Home");

ViewBag.Username = username;

    // 1. Tarih Ayarları
    DateTime secilenTarih = string.IsNullOrEmpty(tarih) ? DateTime.Today : DateTime.Parse(tarih);
    ViewBag.SecilenTarih = secilenTarih.ToString("dd.MM.yyyy");
    ViewBag.TarihParam = secilenTarih.ToString("yyyy-MM-dd");
    
    // Hatalı olan satır burasıydı, alt kısım temizlendi:
    ViewBag.MevcutAyYil = secilenTarih.ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"));
    ViewBag.SecilenAy = secilenTarih.Month;
    ViewBag.SecilenYil = secilenTarih.Year; // Hata veren kısım düzeltildi
    ViewBag.BugunParam = DateTime.Today.ToString("yyyy-MM-dd");

    // ... kodların geri kalan alt kısımlarına (PlanEkle, PlanSil vb.) dokunmayın.

            // 2. Ayın Toplam Gün Sayısını ve İlk Günün Haftanın Hangi Günü Olduğunu Bulma
            int gunSayisi = DateTime.DaysInMonth(secilenTarih.Year, secilenTarih.Month);
            ViewBag.DaysInMonth = gunSayisi;
            
            DateTime ayinIlkGunu = new DateTime(secilenTarih.Year, secilenTarih.Month, 1);
            // Pazartesi=1, Salı=2 ... Pazar=7 olacak şekilde ayarlama
            int baslangicGunu = ((int)ayinIlkGunu.DayOfWeek == 0) ? 7 : (int)ayinIlkGunu.DayOfWeek;
            ViewBag.FirstDayOfWeek = baslangicGunu;

            // 3. Her Gün İçin Plan Sayılarını Veritabanından Çekme (Sözlük/Dictionary olarak)
            // Controllers/DashboardController.cs içindeki ilgili bölümü bulun ve sadece burayı güncelleyin:

// 3. Her Gün İçin Plan Sayılarını Veritabanından Çekme (Düzeltilmiş Hali)
var planSayilari = _context.Gorevler
    .Where(g => g.Tarih.Year == secilenTarih.Year && g.Tarih.Month == secilenTarih.Month)
    .AsEnumerable() // Veritabanı çeviri hatasını engellemek için veriyi hafızaya alıyoruz
    .GroupBy(g => g.Tarih.Date)
    .ToDictionary(group => group.Key, group => group.Count());

ViewBag.PlanSayilari = planSayilari;

            // 4. Seçilen Güne Ait Planları Listeleme
            var secilenStart = secilenTarih.Date;
            var secilenEnd = secilenStart.AddDays(1);
            var planlar = _context.Gorevler.Where(g => g.Tarih >= secilenStart && g.Tarih < secilenEnd).ToList();

            return View("GunlukPlan", planlar);
        }

        [HttpPost]
        public IActionResult PlanEkle(string user, string tarih, string baslik)
        {
            if (!string.IsNullOrEmpty(baslik))
            {
                var yeniGorev = new Gorev
                {
                    Baslik = baslik,
                    Tarih = DateTime.Parse(tarih),
                    IsCompleted = false,
                    KullaniciId = 1
                };

                _context.Gorevler.Add(yeniGorev);
                _context.SaveChanges();
            }

            return RedirectToAction("GunlukPlan", new { user = user, tarih = tarih });
        }

        [HttpPost]
        public IActionResult PlanSil(int id, string user, string tarih)
        {
            var gorev = _context.Gorevler.Find(id);
            if (gorev != null)
            {
                _context.Gorevler.Remove(gorev);
                _context.SaveChanges();
            }

            return RedirectToAction("GunlukPlan", new { user = user, tarih = tarih });
        }

        [HttpPost]
        public IActionResult PlanTamamla(int id, string user, string tarih)
        {
            var gorev = _context.Gorevler.Find(id);
            if (gorev != null)
            {
                gorev.IsCompleted = !gorev.IsCompleted;
                _context.SaveChanges();
            }

            return RedirectToAction("GunlukPlan", new { user = user, tarih = tarih });
        }
        // ---- SU TAKİBİ MODÜLÜ ----

// Controllers/DashboardController.cs içindeki bu metodu bulun ve rotalarını güncelleyin:

// Controllers/DashboardController.cs içindeki bu metodu bulun ve üstündeki [Route] satırlarını aynen böyle yapın:

// Controllers/DashboardController.cs içindeki ilgili metodu bulun ve tam olarak bu şekilde güncelleyin:

// ---- SU TAKİBİ MODÜLÜ (Eksiksiz ve Hatasız Sürüm) ----

[HttpGet]
[Route("Dashboard/SuTakibi")]
[Route("Dashboard/SuTakip")]
public IActionResult SuTakibi(string user, string? tarih)
{
    var username = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Index", "Home");

    ViewBag.Username = username;
    DateTime secilenTarih = string.IsNullOrEmpty(tarih) ? DateTime.Today : DateTime.Parse(tarih);
    ViewBag.SecilenTarih = secilenTarih.ToString("dd.MM.yyyy");
    ViewBag.TarihParam = secilenTarih.ToString("yyyy-MM-dd");

    var waterRecords = _context.Sular
        .Where(s => s.KullaniciId == username)
        .ToList();

    var waterByDateMl = waterRecords
        .GroupBy(s => s.Tarih.Date)
        .ToDictionary(
            g => g.Key.ToString("yyyy-MM-dd"),
            g => g.Sum(s => s.Miktar) * 1000.0
        );

    var model = new DashboardSuTakipViewModel
    {
        Username = username,
        SecilenTarih = secilenTarih,
        WaterByDateMl = waterByDateMl
    };

    return View("SuTakip", model);
}

[HttpPost]


[HttpPost]
public IActionResult SuTemizle(string user, string tarih)
{
    DateTime secilenTarih = DateTime.Parse(tarih);
    var bugunkuSular = _context.Sular.Where(s => s.Tarih >= secilenTarih.Date && s.Tarih < secilenTarih.Date.AddDays(1)).ToList();
    
    _context.Sular.RemoveRange(bugunkuSular);
    _context.SaveChanges();

    return RedirectToAction("SuTakibi", new { user = user, tarih = tarih });
}
// ---- KİTAP TAKİBİ MODÜLÜ ----

[HttpGet]
[Route("Dashboard/KitapTakip")]
public IActionResult KitapTakip()
{
    string? userName = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(userName))
        return RedirectToAction("Index", "Home"); // Değişkene atadık
    ViewBag.Username = userName;

    // LINQ sorgusunu düzgün hale getirdik
    var kitap = _context.Kitaplar.FirstOrDefault(k => k.KullaniciId == userName);
    
    if (kitap == null)
    {
        kitap = new KitapTakip { KitapAdi = "rrrrrr", ToplamSayfa = 320, KalinanSayfa = 0, KullaniciId = userName };
        _context.Kitaplar.Add(kitap);
        _context.SaveChanges();
    }
    return View(kitap);
}

[HttpGet]
[Route("Dashboard/TestQueries")]
public IActionResult TestQueries(string user)
{
   var username = HttpContext.Session.GetString("UserId");

if (string.IsNullOrEmpty(username))
    return RedirectToAction("Index", "Home");
    var today = DateTime.Today;
    var thisMonth = new DateTime(today.Year, today.Month, 1);
    var nextMonth = thisMonth.AddMonths(1);

    var results = new List<string>();

    // 1) Sular last 7 days (single day range test)
    try
    {
        var date = today;
        var dayStart = date.Date;
        var dayEnd = dayStart.AddDays(1);
        var count = _context.Sular.Where(s => s.KullaniciId == username && s.Tarih >= dayStart && s.Tarih < dayEnd).Count();
        results.Add($"Sular OK: {count}");
    }
    catch (Exception ex)
    {
        results.Add($"Sular ERR: {ex.GetType().FullName}: {ex.Message}");
    }

    // 2) Sporlar this month
    try
    {
        var count = _context.Sporlar.Where(s => s.KullaniciId == username).Count();
        results.Add($"Sporlar OK: {count}");
    }
    catch (Exception ex)
    {
        results.Add($"Sporlar ERR: {ex.GetType().FullName}: {ex.Message}");
    }

    // 3) Harcamalar this month
    try
    {
        var count = _context.Harcamalar.Where(h => h.KullaniciId == username && h.Tarih >= thisMonth && h.Tarih < nextMonth).Count();
        results.Add($"Harcamalar OK: {count}");
    }
    catch (Exception ex)
    {
        results.Add($"Harcamalar ERR: {ex.GetType().FullName}: {ex.Message}");
    }

    // 4) Kitap
    try
    {
        var k = _context.Kitaplar.FirstOrDefault(k => k.KullaniciId == username);
        results.Add($"Kitap OK: {(k == null ? "null" : k.KitapAdi)}");
    }
    catch (Exception ex)
    {
        results.Add($"Kitap ERR: {ex.GetType().FullName}: {ex.Message}");
    }

    // 5) Gorevler by parsed userId
    try
    {
        if (int.TryParse(username, out var userId))
        {
            var count = _context.Gorevler.Where(g => g.KullaniciId == userId && g.Tarih >= thisMonth && g.Tarih < nextMonth).Count();
            results.Add($"Gorevler OK (by int): {count}");
        }
        else
        {
            var count = _context.Gorevler.Where(g => g.Tarih >= thisMonth && g.Tarih < nextMonth).Count();
            results.Add($"Gorevler OK (all): {count}");
        }
    }
    catch (Exception ex)
    {
        results.Add($"Gorevler ERR: {ex.GetType().FullName}: {ex.Message}");
    }

    // Extra diagnostic: total Sular count (no user filter) and sample rows
    try
    {
        var totalAllSular = _context.Sular.Count();
        results.Add($"Sular (all) Count: {totalAllSular}");

        var sample = _context.Sular.OrderByDescending(s => s.Tarih).Take(5)
            .Select(s => new { s.Tarih, s.Miktar, s.KullaniciId })
            .ToList();
        foreach (var r in sample)
        {
            results.Add($"Sample: {r.Tarih:yyyy-MM-dd HH:mm} | {r.Miktar} L | user={r.KullaniciId}");
        }
    }
    catch (Exception ex)
    {
        results.Add($"Sular(all) ERR: {ex.GetType().FullName}: {ex.Message}");
    }

    return Content(string.Join("\n", results));
}
[HttpPost]
public IActionResult IlerlemeyiKaydet(int kalinanSayfa, string user)
{
    var kitap = _context.Kitaplar.FirstOrDefault(k => k.KullaniciId == user);
    if (kitap != null)
    {
        kitap.KalinanSayfa = Math.Clamp(kalinanSayfa, 0, kitap.ToplamSayfa);
        _context.SaveChanges();
    }
    // URL'yi sabitleyip aynı sayfayı tekrar çağırıyoruz
    return Redirect($"/Dashboard/KitapTakip?user={user}");
}

[HttpPost]
public IActionResult KitabiGuncelle(string kitapAdi, int toplamSayfa, string user)
{
    var kitap = _context.Kitaplar.FirstOrDefault(k => k.KullaniciId == user);
    if (kitap != null)
    {
        kitap.KitapAdi = kitapAdi;
        kitap.ToplamSayfa = toplamSayfa;
        kitap.KalinanSayfa = Math.Min(kitap.KalinanSayfa, toplamSayfa);
        _context.SaveChanges();
    }
    // URL'yi sabitleyip aynı sayfayı tekrar çağırıyoruz
    return Redirect($"/Dashboard/KitapTakip?user={user}");
}

[HttpGet]
[Route("Dashboard/ParaTakip")]
[Route("Dashboard/Harcama")]
public IActionResult ParaTakip(string? tarih)
{
    var username = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Index", "Home");
    ViewBag.Username = username;

    DateTime selectedDate = string.IsNullOrEmpty(tarih) ? DateTime.Today : DateTime.Parse(tarih);
    ViewBag.TarihParam = selectedDate.ToString("yyyy-MM-dd");
    ViewBag.SelectedDate = selectedDate.ToString("dd.MM.yyyy");

    var expenses = _context.Harcamalar
        .Where(h => h.KullaniciId == username)
        .OrderByDescending(h => h.Tarih)
        .ToList();

    var model = new DashboardParaTakipViewModel
    {
        Username = username,
        Tarih = selectedDate,
        Expenses = expenses,
        TodayTotal = expenses.Where(h => h.Tarih.Date == selectedDate.Date).Sum(h => h.Tutar),
        MonthlyTotal = expenses.Where(h => h.Tarih.Year == selectedDate.Year && h.Tarih.Month == selectedDate.Month).Sum(h => h.Tutar)
    };

    return View("ParaTakip", model);
}

[HttpPost]
public IActionResult HarcamaEkle(string user, string tarih, string aciklama, double tutar)
{
    var username = HttpContext.Session.GetString("UserId");

if (string.IsNullOrEmpty(username))
    return RedirectToAction("Index", "Home");
    if (!string.IsNullOrWhiteSpace(aciklama) && tutar > 0)
    {
        _context.Harcamalar.Add(new Harcama
        {
            Tarih = DateTime.Parse(tarih),
            Aciklama = aciklama,
            Tutar = tutar,
            KullaniciId = username
        });
        _context.SaveChanges();
    }

    return RedirectToAction("ParaTakip", new { user = username, tarih = tarih });
}

[HttpPost]
public IActionResult HarcamaSil(int id, string user, string tarih)
{
    var harcama = _context.Harcamalar.Find(id);
    if (harcama != null)
    {
        _context.Harcamalar.Remove(harcama);
        _context.SaveChanges();
    }

    return RedirectToAction("ParaTakip", new { user = user, tarih = tarih });
}

[HttpPost]
public IActionResult AntrenmanEkle(string gun, string antrenmanAdi, string user)
{
    if (!string.IsNullOrEmpty(antrenmanAdi))
    {
        _context.Sporlar.Add(new SporAntrenman { Gun = gun, AntrenmanAdi = antrenmanAdi, KullaniciId = user });
        _context.SaveChanges();
    }
    return Redirect($"/Dashboard/SporTakip?user={user}");
}
[HttpPost]
public IActionResult AntrenmanSil(int id, string user)
{
    var antrenman = _context.Sporlar.Find(id);
    if (antrenman != null)
    {
        _context.Sporlar.Remove(antrenman);
        _context.SaveChanges();
    }
    return Redirect($"/Dashboard/SporTakip?user={user}");
}
[HttpGet]
[Route("Dashboard/SporTakip")]
public IActionResult SporTakip()
{
    var username = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Index", "Home");
    ViewBag.Username = username;
    string bugunAdi = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("tr-TR")).ToUpper();

    string[] gunler = { "PAZARTESİ", "SALI", "ÇARŞAMBA", "PERŞEMBE", "CUMA", "CUMARTESİ" };
    int todayIndex = Array.FindIndex(gunler, g => g == bugunAdi);

    var tumAntrenmanlar = _context.Sporlar
        .Where(s => s.KullaniciId == username)
        .ToList();

    var viewModel = new DashboardSporTakipViewModel
    {
        CurrentDayName = bugunAdi,
        TodayDayIndex = todayIndex,
        Workouts = tumAntrenmanlar
    };

    return View(viewModel);
}

[HttpGet]
[Route("Dashboard/Istatistik")]
public IActionResult Istatistik(string user)
{
    var username = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Index", "Home");

    ViewBag.Username = username;
    var today = DateTime.Today;
    var thisMonth = new DateTime(today.Year, today.Month, 1);
    var nextMonth = thisMonth.AddMonths(1);

    // Water data for last 7 days
    var waterDataLast7Days = new List<double>();
    var waterDaysLabels = new List<string>();
    for (int i = 6; i >= 0; i--)
    {
        var date = today.AddDays(-i);
        var dayName = date.ToString("ddd", new System.Globalization.CultureInfo("tr-TR"));
        waterDaysLabels.Add(dayName);
        try
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);
            var dayWater = _context.Sular
                .Where(s => s.KullaniciId == username && s.Tarih >= dayStart && s.Tarih < dayEnd)
                .Sum(s => (double?)s.Miktar) ?? 0.0;
            waterDataLast7Days.Add(dayWater);
        }
        catch
        {
            waterDataLast7Days.Add(0.0);
        }
    }

    // Sports data this month
    List<Models.SporAntrenman> sportsThisMonth;
    int totalWorkouts = 0;
    int completedWorkouts = 0;
    try
    {
        sportsThisMonth = _context.Sporlar
            .Where(s => s.KullaniciId == username)
            .ToList();
        totalWorkouts = sportsThisMonth.Count;
        completedWorkouts = sportsThisMonth.Count(s => s.IsCompleted);
    }
    catch
    {
        sportsThisMonth = new List<Models.SporAntrenman>();
    }
    double workoutRate = totalWorkouts == 0 ? 0 : (double)completedWorkouts / totalWorkouts * 100;

    // Expense data this month
    List<Models.Harcama> expensesThisMonth;
    double monthlyExpenseTotal = 0;
    try
    {
        expensesThisMonth = _context.Harcamalar
            .Where(h => h.KullaniciId == username && h.Tarih >= thisMonth && h.Tarih < nextMonth)
            .ToList();
        monthlyExpenseTotal = expensesThisMonth.Sum(h => h.Tutar);
    }
    catch
    {
        expensesThisMonth = new List<Models.Harcama>();
    }
    int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
    double avgDailyExpense = monthlyExpenseTotal / daysInMonth;

    // Book reading progress
    Models.KitapTakip? currentBook = null;
    try
    {
        currentBook = _context.Kitaplar.FirstOrDefault(k => k.KullaniciId == username);
    }
    catch
    {
        currentBook = null;
    }
    string bookName = currentBook?.KitapAdi ?? "Kitap yok";
    int totalPages = currentBook?.ToplamSayfa ?? 0;
    int pagesRead = currentBook?.KalinanSayfa ?? 0;
    double readingProgress = totalPages == 0 ? 0 : (double)pagesRead / totalPages * 100;

    // Tasks data this month
    // Gorev.KullaniciId is an int in the model; attempt to parse username safely
    List<Models.Gorev> tasksThisMonth;
    try
    {
        if (int.TryParse(username, out var userId))
        {
            tasksThisMonth = _context.Gorevler
                .Where(g => g.KullaniciId == userId && g.Tarih >= thisMonth && g.Tarih < nextMonth)
                .ToList();
        }
        else
        {
            tasksThisMonth = _context.Gorevler
                .Where(g => g.Tarih >= thisMonth && g.Tarih < nextMonth)
                .ToList();
        }
    }
    catch
    {
        tasksThisMonth = new List<Models.Gorev>();
    }
    int totalTasks = tasksThisMonth.Count;
    int completedTasks = tasksThisMonth.Count(g => g.IsCompleted);
    double taskRate = totalTasks == 0 ? 0 : (double)completedTasks / totalTasks * 100;

    var model = new DashboardIstatistikViewModel
    {
        Username = username,
        WaterDataLast7Days = waterDataLast7Days,
        WaterDaysLabels = waterDaysLabels,
        WaterAverageLast7Days = waterDataLast7Days.Count > 0 ? waterDataLast7Days.Average() : 0,
        WaterTotalLast7Days = waterDataLast7Days.Sum(),
        TotalWorkoutsThisMonth = totalWorkouts,
        CompletedWorkoutsThisMonth = completedWorkouts,
        WorkoutCompletionRate = workoutRate,
        MonthlyExpenseTotal = monthlyExpenseTotal,
        ExpenseCountThisMonth = expensesThisMonth.Count,
        AverageDailyExpense = avgDailyExpense,
        CurrentBook = bookName,
        TotalPages = totalPages,
        PagesRead = pagesRead,
        ReadingProgress = readingProgress,
        TotalTasksThisMonth = totalTasks,
        CompletedTasksThisMonth = completedTasks,
        TaskCompletionRate = taskRate
    };

    return View(model);
}

[HttpPost]
public IActionResult AntrenmanTamamla(int id, string user)
{
    var antrenman = _context.Sporlar.Find(id);
    if (antrenman != null)
    {
        antrenman.IsCompleted = !antrenman.IsCompleted; // Basınca tamamlandı yap, tekrar basınca geri al
        _context.SaveChanges();
    }
    return Redirect($"/Dashboard/SporTakip?user={user}");
}
    }
}