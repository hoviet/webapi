using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class DSSanPham
    {
        public SanPham sanPham { get; set; }
        public int soLuong { get; set; }
        public float giaKM { get; set; }
        public float tongGia { get; set; }
    }
}