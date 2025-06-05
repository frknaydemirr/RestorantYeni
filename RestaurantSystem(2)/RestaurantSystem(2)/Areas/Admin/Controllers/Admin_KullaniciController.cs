using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantSystem_2_.Models.VT;

namespace RestaurantSystem_2_.Areas.Admin.Controllers
{
    [Authorize]
    public class Admin_KullaniciController : Controller
    {
        private RestaurantSystemEntities db = new RestaurantSystemEntities();

        // GET: Admin/Admin_Kullanici
        public ActionResult Index()
        {
            return View(db.Tbl_Kullanici.ToList());
        }

        // GET: Admin/Admin_Kullanici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Kullanici tbl_Kullanici = db.Tbl_Kullanici.Find(id);
            if (tbl_Kullanici == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Kullanici);
        }

        // GET: Admin/Admin_Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Admin_Kullanici/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,KullaniciAdi,Sifre")] Tbl_Kullanici tbl_Kullanici)
        {
            if (ModelState.IsValid)
            {
                // Şifreyi hashle
                tbl_Kullanici.Sifre = HashPassword(tbl_Kullanici.Sifre);

                try
                {
                    db.Tbl_Kullanici.Add(tbl_Kullanici);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            // Hata mesajını Output'a yaz
                            System.Diagnostics.Debug.WriteLine($"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");

                            // Hata mesajını kullanıcıya göster
                            ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                    // throw; // Artık yeniden fırlatmıyoruz, çünkü hata gösterilecek
                }

            }

            // ModelState geçerli değilse formu geri döndür
            return View(tbl_Kullanici);
        }



        // GET: Admin/Admin_Kullanici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Kullanici tbl_Kullanici = db.Tbl_Kullanici.Find(id);
            if (tbl_Kullanici == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Kullanici);
        }

        // POST: Admin/Admin_Kullanici/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,KullaniciAdi,Sifre")] Tbl_Kullanici tbl_Kullanici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Kullanici).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Kullanici);
        }

        // GET: Admin/Admin_Kullanici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Kullanici tbl_Kullanici = db.Tbl_Kullanici.Find(id);
            if (tbl_Kullanici == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Kullanici);
        }

        // POST: Admin/Admin_Kullanici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Kullanici tbl_Kullanici = db.Tbl_Kullanici.Find(id);
            db.Tbl_Kullanici.Remove(tbl_Kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public JsonResult DeleteKullanici(int id)
        {
            try
            {
                Tbl_Kullanici tbl_Kullanici = db.Tbl_Kullanici.Find(id);
                if (tbl_Kullanici == null)
                {
                    return Json(new { success = false, message = "Kullanıcı   bulunamadı!" });
                }
                db.Tbl_Kullanici.Remove(tbl_Kullanici);
                db.SaveChanges();
                return Json(new { success = true, message = "Kullanıcı Silindi!" });

            }
            catch
            {
                return Json(new { success = false, message = "Hata oluştu" });
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
