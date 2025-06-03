using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantSystem_2_.Models.VT;
using static System.Collections.Specialized.BitVector32;
using System.Web.Mvc;
using System.Web.Security;

namespace RestaurantSystem_2_.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private RestaurantSystemEntities db = new RestaurantSystemEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Tbl_Kullanici model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var hashedPassword = HashPassword(model.Sifre);

            var user = db.Tbl_Kullanici.FirstOrDefault(u => u.KullaniciAdi == model.KullaniciAdi && u.Sifre == hashedPassword);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.KullaniciAdi, false);

                Session["Kullanici"] = user;
                Session["KullaniciAdi"] = user.KullaniciAdi;

                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre!";
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        // SHA256 hash metodu
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // Yeni: Tüm mevcut parolaları hash'ler ve kaydeder
        public ActionResult HashExistingPasswords()
        {
            var users = db.Tbl_Kullanici.ToList();
            int updatedCount = 0;

            foreach (var user in users)
            {
                // Null veya boş parolaları atla
                if (string.IsNullOrEmpty(user.Sifre))
                    continue;

                // Eğer parola 64 karakter değilse VEYA hexadecimal değilse, yeniden hash'le
                if (user.Sifre.Length != 64 || !IsValidSHA256(user.Sifre))
                {
                    user.Sifre = HashPassword(user.Sifre);
                    updatedCount++;
                }
            }

            db.SaveChanges();
            return Content($"{updatedCount} parola güncellendi.");
        }

        // SHA256 hash kontrolü (case-insensitive + hex format)
        private bool IsValidSHA256(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 64)
                return false;

            // Regex ile hexadecimal kontrol (büyük/küçük harf duyarsız)
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[0-9a-fA-F]{64}$");
        }

    }
}