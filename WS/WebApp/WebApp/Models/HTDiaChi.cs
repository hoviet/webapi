using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class HTDiaChi
    {
        public int idDiaChiKhachHang { get; set; }
        public string tenDiaChi { get; set; }
        public string tenKhachHang { get; set; }
        public string soDt { get; set; }
        public int idKhachHang { get; set; }
        public bool loai { get; set; }
    }
}