using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
//using Restorant_Sistem.Models;
//using Restorant_Sistem.Models.ViewModel;
//using Restorant_Sistem.Models.VT;
namespace Restorant_Sistem.Controllers
{
    public class PartialController : Controller
    {
        //private RestaurantSystemEntities db = new RestaurantSystemEntities();
        public PartialViewResult NavbarPartial()
        {

            return PartialView();
        }

        public PartialViewResult FooterPartial()
        {
            //FirmaBilgileriViewModel vm = new FirmaBilgileriViewModel();
            return PartialView();
        }



    }
}