using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class HTDiaChi
    {
        public int idDiaChiKhachHang { get; set; }
        public int idKhachHang { get; set; }
        public string tinh { get; set; }
        public string quanHuyen { get; set; }
        public string xaPhuong { get; set; }
        public string diaChi { get; set; }
        public string tenDiaChi { get; set; }
        public string tenKhachHang { get; set; }
        public string soDt { get; set; }      
        public bool loai { get; set; }
        
    }
}