using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantSystem_2_.Models.VT;
namespace RestaurantSystem_2_.Models.ViewModel
{
    public class IndexViewModel
    {
        public List<Tbl_FirmaBilgileri> Tbl_FirmaBilgiler { get; set; }
        public List<Tbl_Menu> Tbl_Menu { get; set; }

    }
}