using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class DiaChi
    {
        public int id { get; set; }
        public int idKhachHang { get; set; }                       
        public string idXaPhuong { get; set; }
        public string idQuanHuyen { get; set; }
        public string idTinh { get; set; }
        public string diaChi { get; set; }
        public  string soDT { get; set; }
        public string tenKhachHang { get; set; }
        public bool loai { get; set; }

    }
}