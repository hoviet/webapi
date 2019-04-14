using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
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
                List<SanPham> lsp = db.SanPhams.ToList();
                List<dynamic> lhp = new List<dynamic>();
                for (int i = 0; i < ldmsp.Count; i++)
                {
                    List<dynamic> ltam = new List<dynamic>();
                    // lay 4 san pham thuoc danh muc
                    int dem = 0;
                    for (int j = 0; j < lsp.Count; j++)
                    {
                        if (ldmsp[i].id_danh_muc == lsp[j].id_danh_muc && dem < 4 && lsp[i].trang_thai == true)
                        {
                            var sp = new
                            {
                                id_san_pham = lsp[j].id_san_pham,
                                id_danh_muc = lsp[j].id_danh_muc,
                                ten_sp = lsp[j].ten_sp,
                                gia_km = lsp[j].gia_km,
                                gia_sp = lsp[j].gia_sp,
                                mo_ta = lsp[j].mo_ta,
                                phan_tram_km = lsp[j].phan_tram_km,
                                url_hinh_chinh = "http://www.3anhem.somee.com" + lsp[j].url_hinh_chinh,
                                so_luong = lsp[j].so_luong,
                            };
                            ltam.Add(sp);
                            dem = dem + 1;
                        }
                        if(dem >= 4)
                        {
                            break;
                        }
                    }
                    var tam = new
                    {
                        id = ldmsp[i].id_danh_muc, // lay id danh muc san pham
                        tenDanhMuc = ldmsp[i].ten_danh_muc,// lay ten danh muc san pham
                        sanPham = ltam, //                           
                    };
                    lhp.Add(tam);
                }
                return Ok(lhp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
