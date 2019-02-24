using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class HoaDon
    {
        public int id { get; set; }
        public int idKhachHang { get; set; }
        public int idTinhTrang { get; set; }       
        public float tongGia { get; set; }
        public int soDT { get; set; }
        public String ghiChu { get; set; }
        public String noiNhan { get; set; }
        public DateTime ngayLap { get; set; }
    }
}