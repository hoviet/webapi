using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SanPhamYeuThichHT
    {
        public int idYeuThich { get; set; }
        public int idKhachHang { get; set; }
        public int idSanPham { get; set; }
        public KhachHang KhachHang { get; set; }
        public SanPham sanPham { get; set; }
    }
}