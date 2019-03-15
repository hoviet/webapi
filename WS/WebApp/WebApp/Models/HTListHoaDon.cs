using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class HTListHoaDon
    {
        public int id { get; set; }
        public string KhachHang { get; set; }
        public string TinhTrang { get; set; }
        public float tongGia { get; set; }
        public int soDT { get; set; }
        public String ghiChu { get; set; }
        public string DiaChi { get; set; }
        public string ngayLap { get; set; }
    }
}