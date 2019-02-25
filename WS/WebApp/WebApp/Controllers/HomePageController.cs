using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomePageController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        //ham tra ve id danh muc san pham, ten danh muc, list 5 san pham
        [ActionName("home")]
        public IHttpActionResult home()
        {
            try
            {
                List<DanhMucSanPham> ldmsp = db.DanhMucSanPhams.ToList();               
                List<HomePage> lhp = new List<HomePage>();                
                for (int i = 0; i <ldmsp.Count; i++)
                {
                    HomePage tam = new HomePage();
                    List<SanPham> ltam = new List<SanPham>();               
                    tam.tenDanhMuc = ldmsp[i].ten_danh_muc; // lay ten danh muc san pham
                    List<SanPham> lsp = db.SanPhams.Where(x => x.id_danh_muc == ldmsp[i].id_danh_muc).Take(5).ToList(); // lay 5 san pham thuoc danh muc

                    for (int j = 0; j < lsp.Count; j++)
                    {
                        SanPham sp = new SanPham();
                        sp.id_san_pham = lsp[j].id_san_pham;
                        sp.id_danh_muc = lsp[j].id_danh_muc;
                        sp.ten_sp = lsp[j].ten_sp;
                        sp.gia_km = lsp[j].gia_km;
                        sp.gia_sp = lsp[j].gia_sp;
                        sp.mo_ta = lsp[j].mo_ta;
                        sp.phan_tram_km = lsp[j].phan_tram_km;
                        sp.url_hinh_chinh = lsp[j].url_hinh_chinh;
                        sp.so_luong = lsp[j].so_luong;
                        ltam.Add(sp);

                    }
                    tam.sanPham = ltam; //
                    tam.id = ldmsp[i].id_danh_muc; // lay id danh muc san pham                    
                    lhp.Add(tam);
                }
                return Ok(lhp);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
