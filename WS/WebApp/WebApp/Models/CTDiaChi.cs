using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CTDiaChi
    {
        public int id { get; set; }
        public int idKhachHang { get; set; }
        public string tenKhachHang { get; set; }
        public string soDT { get; set; }
        public string idTinh { get; set; }
        public string idQuanHuyen { get; set; }
        public string idXaPhuong { get; set; }
        public string diaChi { get; set; }
        public string tenXaPhuong { get; set; }
        public string tenQuanHuyen { get; set; }
        public string tenTinh { get; set; }
        public List<XaPhuong> dsXaPhuong { get; set; }
        public List<QuanHuyen> dsQuanHuyen { get; set; }
    }
}