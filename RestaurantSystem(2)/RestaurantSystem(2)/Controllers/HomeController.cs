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

        [Route("")]
        [Route("Anasayfa")]
        public ActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault();
            vm.Tbl_Menu = db.Tbl_Menu.ToList();
            vm.Tbl_MenuKategori = db.Tbl_MenuKategori.ToList();
            return View(vm);
        }



        [Route("Menu")]
        public ActionResult Menu()
        {

            var model = new IndexViewModel
            {
                Tbl_MenuKategori = db.Tbl_MenuKategori.ToList(),
                Tbl_Menu = db.Tbl_Menu.ToList()
            };

            // DEBUG: Verileri kontrol edin
            Debug.WriteLine($"Kategori Sayısı: {model.Tbl_MenuKategori.Count}");
            Debug.WriteLine($"Menü Öğe Sayısı: {model.Tbl_Menu.Count}");

            return View(model); 
        }








    }
}