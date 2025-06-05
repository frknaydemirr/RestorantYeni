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
    [Authorize]
    public class Admin_FirmaBilgileriController : Controller
    {
        private RestaurantSystemEntities db = new RestaurantSystemEntities();

        // GET: Admin/Admin_FirmaBilgileri
        public ActionResult Index()
        {
            return View(db.Tbl_FirmaBilgileri.ToList());
        }

        // GET: Admin/Admin_FirmaBilgileri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_FirmaBilgileri tbl_FirmaBilgileri = db.Tbl_FirmaBilgileri.Find(id);
            if (tbl_FirmaBilgileri == null)
            {
                return HttpNotFound();
            }
            return View(tbl_FirmaBilgileri);
        }

        // GET: Admin/Admin_FirmaBilgileri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Admin_FirmaBilgileri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Tbl_FirmaBilgileri model, HttpPostedFileBase Logo, HttpPostedFileBase ResimURL)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage);
                    ViewBag.ErrorMessage = string.Join("<br/>", errors);
                    return View(model);
                }

                // Logo işlemleri
                if (Logo != null && Logo.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var logoExtension = Path.GetExtension(Logo.FileName).ToLower();
                    if (!allowedExtensions.Contains(logoExtension))
                    {
                        ViewBag.ErrorMessage = "Geçersiz logo formatı. Sadece JPG, PNG veya GIF kabul edilir.";
                        return View(model);
                    }

                    var logoUploadPath = Server.MapPath("~/Uploads/FirmaLogolari");
                    if (!Directory.Exists(logoUploadPath))
                        Directory.CreateDirectory(logoUploadPath);

                    var logoFileName = Guid.NewGuid() + logoExtension;
                    var logoFilePath = Path.Combine(logoUploadPath, logoFileName);
                    Logo.SaveAs(logoFilePath);
                    model.Logo = "/Uploads/FirmaLogolari/" + logoFileName;
                }

                // Resim işlemleri
                if (ResimURL != null && ResimURL.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var resimExtension = Path.GetExtension(ResimURL.FileName).ToLower();
                    if (!allowedExtensions.Contains(resimExtension))
                    {
                        if (!string.IsNullOrEmpty(model.Logo))
                        {
                            var logoPath = Server.MapPath(model.Logo);
                            if (System.IO.File.Exists(logoPath))
                                System.IO.File.Delete(logoPath);
                        }
                        ViewBag.ErrorMessage = "Geçersiz resim formatı. Sadece JPG, PNG veya GIF kabul edilir.";
                        return View(model);
                    }

                    var resimUploadPath = Server.MapPath("~/Uploads/FirmaResimleri");
                    if (!Directory.Exists(resimUploadPath))
                        Directory.CreateDirectory(resimUploadPath);

                    var resimFileName = Guid.NewGuid() + resimExtension;
                    var resimFilePath = Path.Combine(resimUploadPath, resimFileName);
                    ResimURL.SaveAs(resimFilePath);
                    model.ResimURL = "/Uploads/FirmaResimleri/" + resimFileName;
                }

                if (string.IsNullOrEmpty(model.Logo) && string.IsNullOrEmpty(model.ResimURL))
                {
                    ViewBag.ErrorMessage = "En az bir resim (logo veya firma resmi) yüklemelisiniz.";
                    return View(model);
                }

                db.Tbl_FirmaBilgileri.Add(model);
                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Firma bilgileri başarıyla kaydedildi";
                    return RedirectToAction("Index", "Admin_FirmaBilgileri", new { area = "Admin" });
                }
                else
                {
                    // Kayıt başarısız olursa yüklenen dosyaları sil
                    if (!string.IsNullOrEmpty(model.Logo))
                    {
                        var logoPath = Server.MapPath(model.Logo);
                        if (System.IO.File.Exists(logoPath))
                            System.IO.File.Delete(logoPath);
                    }

                    if (!string.IsNullOrEmpty(model.ResimURL))
                    {
                        var resimPath = Server.MapPath(model.ResimURL);
                        if (System.IO.File.Exists(resimPath))
                            System.IO.File.Delete(resimPath);
                    }

                    ViewBag.ErrorMessage = "Kayıt işlemi başarısız oldu.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda yüklenen dosyaları sil
                if (!string.IsNullOrEmpty(model.Logo))
                {
                    var logoPath = Server.MapPath(model.Logo);
                    if (System.IO.File.Exists(logoPath))
                        System.IO.File.Delete(logoPath);
                }

                if (!string.IsNullOrEmpty(model.ResimURL))
                {
                    var resimPath = Server.MapPath(model.ResimURL);
                    if (System.IO.File.Exists(resimPath))
                        System.IO.File.Delete(resimPath);
                }

                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(model);
            }
        }

        // GET: Admin/Admin_FirmaBilgileri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_FirmaBilgileri tbl_FirmaBilgileri = db.Tbl_FirmaBilgileri.Find(id);
            if (tbl_FirmaBilgileri == null)
            {
                return HttpNotFound();
            }
            return View(tbl_FirmaBilgileri);
        }

        // POST: Admin/Admin_FirmaBilgileri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Tbl_FirmaBilgileri model, HttpPostedFileBase logoFile, HttpPostedFileBase imageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage);
                    ViewBag.ErrorMessage = string.Join("<br/>", errors);
                    return View(model);
                }

                // Mevcut kaydı veritabanından al
                var existingFirma = db.Tbl_FirmaBilgileri.Find(model.ID);
                if (existingFirma == null)
                {
                    ViewBag.ErrorMessage = "Düzenlenmek istenen firma bulunamadı.";
                    return View(model);
                }

                string oldLogoPath = existingFirma.Logo;
                string oldResimPath = existingFirma.ResimURL;

                // Logo işlemleri
                if (logoFile != null && logoFile.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var logoExtension = Path.GetExtension(logoFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(logoExtension))
                    {
                        ViewBag.ErrorMessage = "Geçersiz logo formatı. Sadece JPG, PNG veya GIF kabul edilir.";
                        return View(model);
                    }

                    var logoUploadPath = Server.MapPath("~/Uploads/FirmaLogolari");
                    if (!Directory.Exists(logoUploadPath))
                        Directory.CreateDirectory(logoUploadPath);

                    var logoFileName = Guid.NewGuid() + logoExtension;
                    var logoFilePath = Path.Combine(logoUploadPath, logoFileName);
                    logoFile.SaveAs(logoFilePath);
                    model.Logo = "/Uploads/FirmaLogolari/" + logoFileName;

                    // Eski logo dosyasını sil
                    if (!string.IsNullOrEmpty(oldLogoPath))
                    {
                        var oldLogoFullPath = Server.MapPath(oldLogoPath);
                        if (System.IO.File.Exists(oldLogoFullPath))
                            System.IO.File.Delete(oldLogoFullPath);
                    }
                }
                else
                {
                    // Logo yüklenmemişse, mevcut logoyu koru
                    model.Logo = oldLogoPath;
                }

                // Resim işlemleri
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var resimExtension = Path.GetExtension(imageFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(resimExtension))
                    {
                        // Hata durumunda yeni yüklenen logoyu sil
                        if (logoFile != null && logoFile.ContentLength > 0 && !string.IsNullOrEmpty(model.Logo))
                        {
                            var newLogoPath = Server.MapPath(model.Logo);
                            if (System.IO.File.Exists(newLogoPath))
                                System.IO.File.Delete(newLogoPath);
                        }
                        ViewBag.ErrorMessage = "Geçersiz resim formatı. Sadece JPG, PNG veya GIF kabul edilir.";
                        return View(model);
                    }

                    var resimUploadPath = Server.MapPath("~/Uploads/FirmaResimleri");
                    if (!Directory.Exists(resimUploadPath))
                        Directory.CreateDirectory(resimUploadPath);

                    var resimFileName = Guid.NewGuid() + resimExtension;
                    var resimFilePath = Path.Combine(resimUploadPath, resimFileName);
                    imageFile.SaveAs(resimFilePath);
                    model.ResimURL = "/Uploads/FirmaResimleri/" + resimFileName;

                    // Eski resim dosyasını sil
                    if (!string.IsNullOrEmpty(oldResimPath))
                    {
                        var oldResimFullPath = Server.MapPath(oldResimPath);
                        if (System.IO.File.Exists(oldResimFullPath))
                            System.IO.File.Delete(oldResimFullPath);
                    }
                }
                else
                {
                    // Resim yüklenmemişse, mevcut resmi koru
                    model.ResimURL = oldResimPath;
                }

                // En az bir resim (logo veya firma resmi) kontrolü
                if (string.IsNullOrEmpty(model.Logo) && string.IsNullOrEmpty(model.ResimURL))
                {
                    ViewBag.ErrorMessage = "En az bir resim (logo veya firma resmi) yüklemelisiniz.";
                    return View(model);
                }

                // Modeli güncelle
                db.Entry(existingFirma).CurrentValues.SetValues(model);
                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Firma bilgileri başarıyla güncellendi";
                    return RedirectToAction("Index", "Admin_FirmaBilgileri", new { area = "Admin" });
                }
                else
                {
                    // Kayıt başarısız olursa yüklenen yeni dosyaları sil
                    if (logoFile != null && logoFile.ContentLength > 0 && !string.IsNullOrEmpty(model.Logo))
                    {
                        var newLogoPath = Server.MapPath(model.Logo);
                        if (System.IO.File.Exists(newLogoPath))
                            System.IO.File.Delete(newLogoPath);
                    }

                    if (imageFile != null && imageFile.ContentLength > 0 && !string.IsNullOrEmpty(model.ResimURL))
                    {
                        var newResimPath = Server.MapPath(model.ResimURL);
                        if (System.IO.File.Exists(newResimPath))
                            System.IO.File.Delete(newResimPath);
                    }

                    ViewBag.ErrorMessage = "Güncelleme işlemi başarısız oldu.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda yüklenen yeni dosyaları sil
                if (logoFile != null && logoFile.ContentLength > 0 && !string.IsNullOrEmpty(model.Logo))
                {
                    var newLogoPath = Server.MapPath(model.Logo);
                    if (System.IO.File.Exists(newLogoPath))
                        System.IO.File.Delete(newLogoPath);
                }

                if (imageFile != null && imageFile.ContentLength > 0 && !string.IsNullOrEmpty(model.ResimURL))
                {
                    var newResimPath = Server.MapPath(model.ResimURL);
                    if (System.IO.File.Exists(newResimPath))
                        System.IO.File.Delete(newResimPath);
                }

                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(model);
            }
        }

        // GET: Admin/Admin_FirmaBilgileri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_FirmaBilgileri tbl_FirmaBilgileri = db.Tbl_FirmaBilgileri.Find(id);
            if (tbl_FirmaBilgileri == null)
            {
                return HttpNotFound();
            }
            return View(tbl_FirmaBilgileri);
        }

        // POST: Admin/Admin_FirmaBilgileri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_FirmaBilgileri tbl_FirmaBilgileri = db.Tbl_FirmaBilgileri.Find(id);
            db.Tbl_FirmaBilgileri.Remove(tbl_FirmaBilgileri);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteFirmaBilgileri(int id)
        {
            try
            {
                Tbl_FirmaBilgileri tbl_FirmaBilgileri = db.Tbl_FirmaBilgileri.Find(id);
                if (tbl_FirmaBilgileri == null)
                {
                    return Json(new { success = false, message = "Firma  bilgisi bulunamadı!" });
                }
                db.Tbl_FirmaBilgileri.Remove(tbl_FirmaBilgileri);
                db.SaveChanges();
                return Json(new { success = true, message = "Firma bilgisi  Silindi!" });

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
