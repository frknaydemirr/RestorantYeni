using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantSystem_2_.Models.ViewModel;
using RestaurantSystem_2_.Models.VT;
namespace Restorant_Sistem.Controllers
{


    public class HomeController : Controller
    {

       public RestaurantSystemEntities db = new RestaurantSystemEntities();

        //[Route("")]
        //[Route("Anasayfa")]
        //public ActionResult Index()
        //{
        //    IndexViewModel vm = new IndexViewModel();
        //    vm.Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault();
        //    vm.Tbl_Menu = db.Tbl_Menu.ToList();
        //    vm.Tbl_MenuKategori = db.Tbl_MenuKategori.ToList();
        //    return View(vm);
        //}

        [Route("")]
        [Route("Anasayfa")]
        public ActionResult Index(string slug)
        {
            var model = new IndexViewModel
            {
                Tbl_MenuKategori = db.Tbl_MenuKategori.ToList(),
                Tbl_Menu = db.Tbl_Menu.ToList(),
                Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault()
            };

            ViewBag.Slug = slug; // Razor'da eşleştirme için
            return View(model);
        }


        [Route("Menu/{Id?}")]
        public ActionResult Menu(int Id)
        {
            // altkategori listesini Id üzerinden alıyorsunuz gibi anladım, ama sizin kodda 'slug' var, onun yerine 'Id' kullanmalı:
            var altkategori = db.Tbl_MenuKategori.Where(x => x.UstKategoriID == Id).ToList();

            if (altkategori.Count > 0)
            {
                // altkategori 0'dan büyükse başka bir action'a yönlendir
                return RedirectToAction("AltKategoriDetay", new { id = Id });
            }
            else
            {
                // 0 ise başka bir action'a yönlendir
                return RedirectToAction("UrunListesi", new { id = Id });
            }
        }
        public ActionResult AltKategoriDetay(int Id)
        {
            var model = new IndexViewModel
            {
                Tbl_MenuKategori = db.Tbl_MenuKategori.Where(x=>x.UstKategoriID==Id).ToList(), 
                Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault(),
                Tbl_SeciliKategori = db.Tbl_MenuKategori.FirstOrDefault(x => x.ID == Id)
            };
            return View(model);
        }
        public ActionResult UrunListesi(int Id)
        {
            var model = new IndexViewModel
            { 
                Tbl_Menu = db.Tbl_Menu.Where(x=>x.KategoriID==Id).ToList(),
                Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault(),
                Tbl_SeciliKategori = db.Tbl_MenuKategori.FirstOrDefault(x => x.ID == Id)
            }; 
            return View(model);
        }










    }
}