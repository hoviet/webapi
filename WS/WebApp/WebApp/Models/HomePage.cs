using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class HomePage
    {
        public int id { get; set; }
        public String tenDanhMuc { get; set; }
        public List<SanPham> sanPham { get; set; }
    }
}