using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantSystem_2_.Models.VT;

namespace RestaurantSystem_2_.Areas.Admin.Controllers
{

    public class Admin_MenuController : Controller
    {
        private RestaurantSystemEntities db = new RestaurantSystemEntities();

        // GET: Admin/Admin_Menu
        public ActionResult Index()
        {
            var tbl_Menu = db.Tbl_Menu.Include(t => t.Tbl_MenuKategori);
            return View(tbl_Menu.ToList());
        }

        // GET: Admin/Admin_Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            if (tbl_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Menu);
        }

        // GET: Admin/Admin_Menu/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd");
            return View();
        }

        // POST: Admin/Admin_Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Menu tbl_Menu, HttpPostedFileBase ResimURL = null)
        {
            try
            {
                // 1. Model validasyonu
                if (!ModelState.IsValid)
                {
                    ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                    return View(tbl_Menu);
                }

                // 2. Resim yükleme zorunluluğu
                if (ResimURL == null || ResimURL.ContentLength == 0)
                {
                    ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                    ViewBag.ErrorMessage = "Lütfen bir yemek resmi seçiniz";
                    return View(tbl_Menu);
                }

                // 3. Resim boyutu kontrolü (5MB)
                if (ResimURL.ContentLength > 5 * 1024 * 1024)
                {
                    ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                    ViewBag.ErrorMessage = "Resim boyutu 5MB'tan büyük olamaz";
                    return View(tbl_Menu);
                }

                // 4. Geçerli resim uzantıları kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }; // webp eklendi
                var extension = Path.GetExtension(ResimURL.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                    ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz";
                    return View(tbl_Menu);
                }

                // 5. Klasör işlemleri
                var uploadPath = Server.MapPath("~/Uploads/Menu");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // 6. Resmi kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadPath, fileName);
                ResimURL.SaveAs(filePath);
                tbl_Menu.ResimURL = "/Uploads/Menu/" + fileName;

                // 7. Veritabanına kaydet
                db.Tbl_Menu.Add(tbl_Menu);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                ViewBag.ErrorMessage = "Kayıt oluşturulurken hata oluştu: " + ex.Message;
                return View(tbl_Menu);
            }
        }

        // GET: Admin/Admin_Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            if (tbl_Menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
            return View(tbl_Menu);
        }

        // POST: Admin/Admin_Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Menu tbl_Menu, HttpPostedFileBase ResimURL = null)
        {
            try
            {
                // 1. Model validasyonu
                if (!ModelState.IsValid)
                {
                    ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                    return View(tbl_Menu);
                }

                // 2. Resim güncelleme işlemi
                if (ResimURL != null && ResimURL.ContentLength > 0)
                {
                    // 3. Resim format ve boyut kontrolü
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(ResimURL.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                        ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz";
                        return View(tbl_Menu);
                    }

                    if (ResimURL.ContentLength > 5 * 1024 * 1024) // 5MB
                    {
                        ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                        ViewBag.ErrorMessage = "Resim boyutu 5MB'tan büyük olamaz";
                        return View(tbl_Menu);
                    }

                    // 4. Klasör kontrolü
                    var uploadPath = Server.MapPath("~/Uploads/Menu");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    // 5. Yeni resmi kaydet
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadPath, fileName);
                    ResimURL.SaveAs(filePath);

                    // 6. Eski resmi sil (opsiyonel)
                    if (!string.IsNullOrEmpty(tbl_Menu.ResimURL))
                    {
                        var oldFilePath = Server.MapPath(tbl_Menu.ResimURL);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    tbl_Menu.ResimURL = "/Uploads/Menu/" + fileName;
                }
                else
                {
                    // Resim değişmemişse mevcut URL'yi koru
                    var existingEntity = db.Tbl_Menu.AsNoTracking().FirstOrDefault(m => m.ID == tbl_Menu.ID);
                    if (existingEntity != null)
                    {
                        tbl_Menu.ResimURL = existingEntity.ResimURL;
                    }
                }

                // 7. Veritabanını güncelle
                db.Entry(tbl_Menu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.KategoriID = new SelectList(db.Tbl_MenuKategori, "ID", "KategoriAd", tbl_Menu.KategoriID);
                ViewBag.ErrorMessage = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return View(tbl_Menu);
            }
        }

        // GET: Admin/Admin_Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            if (tbl_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Menu);
        }

        // POST: Admin/Admin_Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            db.Tbl_Menu.Remove(tbl_Menu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult DeleteMenu(int id)
        {
            try
            {
                Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
                if (tbl_Menu == null)
                {
                    return Json(new { success = false, message = "Menü bulunamadı!" });
                }
                db.Tbl_Menu.Remove(tbl_Menu);
                db.SaveChanges();
                return Json(new { success = true, message = "Menü Silindi!" });

            }
            catch
            {
                return Json(new { success = false, message = "Hata oluştu" });
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
