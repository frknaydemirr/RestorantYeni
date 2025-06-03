using System;
using System.Collections.Generic;
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
            vm.Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.ToList();
            vm.Tbl_Menu = db.Tbl_Menu.ToList(); 
            return View();
        }



        [Route("Menu")]
        public ActionResult Menu()
        {

            MenuViewModel vm = new MenuViewModel();
            vm.Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.ToList();
            vm.Tbl_Menu = db.Tbl_Menu.ToList();
            vm.Tbl_MenuKategori = db.Tbl_MenuKategori.ToList();
            return View();
        }








    }
}