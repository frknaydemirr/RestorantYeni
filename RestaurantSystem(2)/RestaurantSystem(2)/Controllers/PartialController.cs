using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RestaurantSystem_2_.Models.ViewModel;
using RestaurantSystem_2_.Models.VT;
//using Restorant_Sistem.Models;
//using Restorant_Sistem.Models.ViewModel;
//using Restorant_Sistem.Models.VT;
namespace Restorant_Sistem.Controllers
{
    public class PartialController : Controller
    {
        private RestaurantSystemEntities db = new RestaurantSystemEntities();
        public PartialViewResult NavbarPartial()
        {
            NavbarViewModel vm = new NavbarViewModel();
            vm.Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault();
            return PartialView(vm);
        }

        public PartialViewResult FooterPartial()
        {
            FooterViewModel vm = new FooterViewModel();
            vm.Tbl_FirmaBilgiler = db.Tbl_FirmaBilgileri.FirstOrDefault();
            return PartialView(vm);
        }



    }
}