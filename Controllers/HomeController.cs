using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MultiTrack.Models;


namespace MultiTrack.Controllers
{
    // Kayıt olan kullanıcıların bilgilerini RAM'de saklamak için geçici bir model
    public class GecciciKullanici
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class HomeController : Controller
    {
        private readonly MultiTrackDbContext _context;

public HomeController(MultiTrackDbContext context)
{
    _context = context;
}

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        // HAFIZA ALANI: Kayıt olan herkes bu listeye yazılır ve proje kapanana kadar siliniz.
        // admin@multitrack.com hesabı tamamen silindi, artık sadece kendi kayıt ettiğin isimler geçerli.
        

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string actionType, string? email, string? password, string? registerEmail, string? registerPassword, string? confirmPassword)
        {
            // --- 1. KAYIT İŞLEMİ ---
            if (actionType == "register")
            {
                if (string.IsNullOrEmpty(registerEmail) || string.IsNullOrEmpty(registerPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    ViewBag.ErrorMessageKey = "ErrorFillAll";
                    ViewBag.ActiveTab = "register";
                    return View();
                }

                if (registerPassword != confirmPassword)
                {
                    ViewBag.ErrorMessageKey = "ErrorPasswordsMismatch";
                    ViewBag.ActiveTab = "register";
                    return View();
                }

                string temizEmail = registerEmail.Trim();

                // Bu e-posta daha önce eklenmiş mi kontrol et
                bool varMi = _context.Kullanici
    .Any(x => x.Email.ToLower() == temizEmail.ToLower());

                if (varMi)
                {
                    ViewBag.ErrorMessageKey = "ErrorEmailExists";
                    ViewBag.ActiveTab = "register";
                    return View();
                }

                // Yeni kullanıcıyı hafızadaki listeye ekle
                var yeniKullanici = new Kullanici
{
    Email = temizEmail,
    Password = HashPassword(registerPassword)
};

_context.Kullanici.Add(yeniKullanici);
_context.SaveChanges();

                ViewBag.SuccessMessageKey = "RegisterSuccess";
                ViewBag.ActiveTab = "login"; // Otomatik giriş sekmesine atar
                return View();
            }

            // --- 2. GİRİŞ İŞLEMİ ---
            if (actionType == "login")
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ViewBag.ErrorMessageKey = "ErrorEmailPassword";
                    ViewBag.ActiveTab = "login";
                    return View();
                }

                string girilenEmail = email.Trim();

                // GİRİŞ KONTROLÜ: Girilen e-posta sistemde var mı?
                var bulunanKullanici = _context.Kullanici
                    .FirstOrDefault(x => x.Email.ToLower() == girilenEmail.ToLower());

                if (bulunanKullanici != null)
                {
                    bool isPasswordCorrect = false;

                    if (bulunanKullanici.Password == password)
                    {
                        // Düz metin şifre eşleşti (Eski kullanıcı). Şifresini hashleyip güncelleyelim.
                        isPasswordCorrect = true;
                        bulunanKullanici.Password = HashPassword(password);
                        _context.SaveChanges();
                    }
                    else if (bulunanKullanici.Password == HashPassword(password))
                    {
                        // Şifre zaten hashlenmiş ve doğru
                        isPasswordCorrect = true;
                    }

                    if (isPasswordCorrect)
                    {
                        HttpContext.Session.SetString("UserId", bulunanKullanici.Id.ToString());
                        return RedirectToAction("Index", "Dashboard");
                    }
                }

                // Kullanıcı listede yoksa veya şifre yanlışsa hata ver
                ViewBag.ErrorMessageKey = "ErrorInvalidLogin";
                ViewBag.ActiveTab = "login";
                return View();
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SetLanguage(string lang, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = LanguageHelper.DefaultLang;
            }

            lang = lang.ToUpperInvariant();
            if (!LanguageHelper.SupportedLanguages.Contains(lang))
            {
                lang = LanguageHelper.DefaultLang;
            }

            Response.Cookies.Append("lang", lang, new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                Path = "/",
                IsEssential = true
            });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
    }
}