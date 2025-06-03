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
        public ActionResult Create([Bind(Include = "ID,Adres,InstagramURL,FacebookURL,TwitterURL,TelNo,Logo,Eposta,ResimURL")] Tbl_FirmaBilgileri tbl_FirmaBilgileri)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_FirmaBilgileri.Add(tbl_FirmaBilgileri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_FirmaBilgileri);
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
        public ActionResult Edit([Bind(Include = "ID,Adres,InstagramURL,FacebookURL,TwitterURL,TelNo,Logo,Eposta,ResimURL")] Tbl_FirmaBilgileri tbl_FirmaBilgileri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_FirmaBilgileri).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_FirmaBilgileri);
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
