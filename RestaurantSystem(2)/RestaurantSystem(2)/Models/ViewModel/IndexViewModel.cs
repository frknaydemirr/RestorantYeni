using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantSystem_2_.Models.VT;
namespace RestaurantSystem_2_.Models.ViewModel
{
    public class IndexViewModel
    {
        public Tbl_FirmaBilgileri Tbl_FirmaBilgiler { get; set; }
        public List<Tbl_Menu> Tbl_Menu { get; set; }

        public List<Tbl_MenuKategori> Tbl_MenuKategori { get; set; }
        public Tbl_MenuKategori Tbl_SeciliKategori { get; set; }

    }
}