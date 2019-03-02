using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TaoDonHang
    {
        public int idKhachHang { get; set; }
        public int idTinhTrang { get; set; }
        public float tongTien { get; set; }
        public DateTime ngayLap { get; set; }
        public int soDT { get; set; }
        public int idNoiNhan { get; set; }
        public String ghiChu { get; set; }
        public List<TaoSanPhamDonHang> danhSachSanPham { get; set; }
    }
}