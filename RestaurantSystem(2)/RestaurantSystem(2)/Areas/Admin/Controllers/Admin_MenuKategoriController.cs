using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantSystem_2_.Models.VT;

namespace RestaurantSystem_2_.Areas.Admin.Controllers
{

    [Authorize]
    public class Admin_MenuKategoriController : Controller
    {
        private RestaurantSystemEntities db = new RestaurantSystemEntities();

        // GET: Admin/Admin_MenuKategori
        public ActionResult Index()
        {
            return View(db.Tbl_MenuKategori.ToList());
        }

        // GET: Admin/Admin_MenuKategori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuKategori tbl_MenuKategori = db.Tbl_MenuKategori.Find(id);
            if (tbl_MenuKategori == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuKategori);
        }

        // GET: Admin/Admin_MenuKategori/Create
        public ActionResult Create()
        {
            var kategoriler = db.Tbl_MenuKategori
                .Select(k => new SelectListItem
                {
                    Value = k.ID.ToString(),
                    Text = k.KategoriAd
                })
                .ToList();

            // En üste "Ana Kategori" seçeneğini ekle
            kategoriler.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "Ana Kategori (Üst Kategori Yok)"
            });

            ViewBag.KategoriList = new SelectList(kategoriler, "Value", "Text");

            return View();
        }


        // POST: Admin/Admin_MenuKategori/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_MenuKategori tbl_MenuKategori, HttpPostedFileBase ResimURL = null)
        {
            try
            {
                // Model validasyonu
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Debug.WriteLine(error.ErrorMessage);
                    }
                    return View(tbl_MenuKategori);
                }

                // Resim kontrolü
                if (ResimURL == null || ResimURL.ContentLength == 0)
                {
                    ModelState.AddModelError("ResimURL", "Lütfen bir kategori resmi seçiniz");
                    return View(tbl_MenuKategori);
                }

                // Resim boyutu kontrolü
                if (ResimURL.ContentLength > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ResimURL", "Resim boyutu 5MB'tan büyük olamaz");
                    return View(tbl_MenuKategori);
                }

                // Uzantı kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ResimURL.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ResimURL", "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz");
                    return View(tbl_MenuKategori);
                }

                // Klasör işlemleri
                var uploadPath = Server.MapPath("~/Uploads/MenuKategori");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Resmi kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadPath, fileName);
                ResimURL.SaveAs(filePath);
                tbl_MenuKategori.ResimURL = "/Uploads/MenuKategori/" + fileName;

                // Veritabanı işlemleri
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Null olabilecek alanları kontrol et
                        if (tbl_MenuKategori.UstKategoriID == 0) // Dropdown'da 0 değeri "Ana Kategori" seçeneği
                        {
                            tbl_MenuKategori.UstKategoriID = null;
                        }

                        db.Tbl_MenuKategori.Add(tbl_MenuKategori);
                        db.SaveChanges();
                        transaction.Commit();

                        TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu!";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Veritabanı hatası: " + ex.Message);
                        // Resmi sil
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        return View(tbl_MenuKategori);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorDetails = ex.Message;
                if (ex.InnerException != null)
                {
                    errorDetails += " | Inner Exception: " + ex.InnerException.Message;
                }
                ModelState.AddModelError("", "Kayıt oluşturulurken hata oluştu: " + errorDetails);
                return View(tbl_MenuKategori);
            }
        }

        // GET: Admin/Admin_MenuKategori/Edit/5
        public ActionResult Edit(int id)
        {
            var model = db.Tbl_MenuKategori.Find(id);

            // Üst Kategorileri seçilebilir liste olarak hazırla (mevcut kategoriyi ve null olanları hariç tut)
            ViewBag.UstKategoriID = new SelectList(
                db.Tbl_MenuKategori.Where(x => x.ID != id && x.UstKategoriID == null),
                "ID",
                "KategoriAd",
                model.UstKategoriID
            );

            return View(model);
        }

        // POST: Admin/Admin_MenuKategori/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_MenuKategori tbl_MenuKategori, HttpPostedFileBase ResimURL = null)
        {
            try
            {
                // 1. Model validasyonu
                if (!ModelState.IsValid)
                {
                    return View(tbl_MenuKategori);
                }

                // 2. Resim güncelleme işlemi
                if (ResimURL != null && ResimURL.ContentLength > 0)
                {
                    // 3. Resim format ve boyut kontrolü
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(ResimURL.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz";
                        return View(tbl_MenuKategori);
                    }

                    if (ResimURL.ContentLength > 5 * 1024 * 1024) // 5MB
                    {
                        ViewBag.ErrorMessage = "Resim boyutu 5MB'tan büyük olamaz";
                        return View(tbl_MenuKategori);
                    }

                    // 4. Klasör kontrolü
                    var uploadPath = Server.MapPath("~/Uploads/MenuKategori");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    // 5. Yeni resmi kaydet
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadPath, fileName);
                    ResimURL.SaveAs(filePath);

                    // 6. Eski resmi sil (opsiyonel)
                    if (!string.IsNullOrEmpty(tbl_MenuKategori.ResimURL))
                    {
                        var oldFilePath = Server.MapPath(tbl_MenuKategori.ResimURL);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    tbl_MenuKategori.ResimURL = "/Uploads/MenuKategori/" + fileName;
                }
                else
                {
                    // Resim değişmemişse mevcut URL'yi koru
                    var existingEntity = db.Tbl_MenuKategori.AsNoTracking().FirstOrDefault(m => m.ID == tbl_MenuKategori.ID);
                    if (existingEntity != null)
                    {
                        tbl_MenuKategori.ResimURL = existingEntity.ResimURL;
                    }
                }

                // 7. Veritabanını güncelle
                db.Entry(tbl_MenuKategori).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return View(tbl_MenuKategori);
            }
        }

        // GET: Admin/Admin_MenuKategori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuKategori tbl_MenuKategori = db.Tbl_MenuKategori.Find(id);
            if (tbl_MenuKategori == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuKategori);
        }

        [HttpPost]
        public JsonResult DeleteMenuKategori(int id)
        {
            try
            {
                Tbl_MenuKategori tbl_MenuKategori = db.Tbl_MenuKategori.Find(id);
                if (tbl_MenuKategori == null)
                {
                    return Json(new { success = false, message = "Menü Kategori bulunamadı!" });
                }
                db.Tbl_MenuKategori.Remove(tbl_MenuKategori);
                db.SaveChanges();
                return Json(new { success = true, message = "Menü Kategori Silindi!" });

            }
            catch
            {
                return Json(new { success = false, message = "Hata oluştu" });
            }
        }





        // POST: Admin/Admin_MenuKategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_MenuKategori tbl_MenuKategori = db.Tbl_MenuKategori.Find(id);
            db.Tbl_MenuKategori.Remove(tbl_MenuKategori);
            db.SaveChanges();
            return RedirectToAction("Index");
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
