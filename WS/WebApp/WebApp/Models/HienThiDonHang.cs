using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class HienThiDonHang
    {
        public int idDonDatHang { get; set; }
        public String trangThai { get; set; }
        public String tenNguoiNhan { get; set; }
        public String soDT { get; set; }
        public String diaChi { get; set; }
        public DateTime ngayLap { get; set; }
        public List<DSSanPham> danhSachHang { get; set; }
    }
}