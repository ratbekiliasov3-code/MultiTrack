using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // HAFIZA ALANI: Kayıt olan herkes bu listeye yazılır ve proje kapanana kadar siliniz.
        // admin@multitrack.com hesabı tamamen silindi, artık sadece kendi kayıt ettiğin isimler geçerli.
        private static readonly List<GecciciKullanici> _kullaniciListesi = new List<GecciciKullanici>();

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
                    ViewBag.ErrorMessage = "Lütfen tüm alanları doldurun.";
                    ViewBag.ActiveTab = "register";
                    return View();
                }

                if (registerPassword != confirmPassword)
                {
                    ViewBag.ErrorMessage = "Şifreler birbiriyle uyuşmuyor!";
                    ViewBag.ActiveTab = "register";
                    return View();
                }

                string temizEmail = registerEmail.Trim();

                // Bu e-posta daha önce eklenmiş mi kontrol et
                bool varMi = false;
                foreach (var u in _kullaniciListesi)
                {
                    if (u.Email.Equals(temizEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        varMi = true;
                        break;
                    }
                }

                if (varMi)
                {
                    ViewBag.ErrorMessage = "Bu e-posta adresiyle zaten bir hesap var!";
                    ViewBag.ActiveTab = "register";
                    return View();
                }

                // Yeni kullanıcıyı hafızadaki listeye ekle
                _kullaniciListesi.Add(new GecciciKullanici
                {
                    Email = temizEmail,
                    Password = registerPassword
                });

                ViewBag.SuccessMessage = "Kayıt işlemi başarılı! Şimdi kendi bilgilerinizle giriş yapabilirsiniz.";
                ViewBag.ActiveTab = "login"; // Otomatik giriş sekmesine atar
                return View();
            }

            // --- 2. GİRİŞ İŞLEMİ ---
            if (actionType == "login")
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ViewBag.ErrorMessage = "E-posta ve şifre alanları boş bırakılamaz.";
                    ViewBag.ActiveTab = "login";
                    return View();
                }

                string girilenEmail = email.Trim();

                // GİRİŞ KONTROLÜ: Girilen e-posta ve şifre hafızadaki listede var mı?
                GecciciKullanici bulunanKullanici = null;
                foreach (var u in _kullaniciListesi)
                {
                    if (u.Email.Equals(girilenEmail, StringComparison.OrdinalIgnoreCase) && u.Password == password)
                    {
                        bulunanKullanici = u;
                        break;
                    }
                }

if (bulunanKullanici != null)
{
    string username = girilenEmail.Split('@')[0];

    HttpContext.Session.SetString("UserId", username);

    return RedirectToAction("Index", "Dashboard");
}

                // Kullanıcı listede yoksa veya şifre yanlışsa hata ver
                ViewBag.ErrorMessage = "Hatalı e-posta veya şifre girdiniz!";
                ViewBag.ActiveTab = "login";
                return View();
            }

            return View();
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