using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ChiTietHoaDon
    {
        public int id { get; set; }
        public int idHoaDon { get; set; }
        public int idSanPham { get; set; }
        public float giaKhuyenMai { get; set; }
        public int soLuong { get; set; }
        public float tongGia { get; set; }
        public DateTime ngayLap { get; set; }
    }
}