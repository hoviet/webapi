using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PagedList;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class SanPhamYeuThichController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("layMot")]
        public IHttpActionResult laySanPhamYeuThich(int id)//id khach hang
        {
            try
            {
                List<SanPhamYeuThich> list = db.SanPhamYeuThiches.Where(x => x.id_khach_hang == id).ToList();               
                List<dynamic> tam = new List<dynamic>();
                if (list.Count ==0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    SanPham sp = db.SanPhams.FirstOrDefault(e => e.id_san_pham == list[i].id_san_pham);
                    var spTam = new
                    {
                        id_san_pham = sp.id_san_pham,
                        id_danh_muc = sp.id_danh_muc,
                        ten_sp = sp.ten_sp,
                        so_luong = sp.so_luong,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + sp.url_hinh_chinh,
                        mo_ta = sp.mo_ta,
                        phan_tram_km = sp.phan_tram_km,
                        gia_sp = sp.gia_sp,
                        gia_km = sp.gia_km
                    };
                    var spyt = new
                    {
                        idKhachHang = list[i].id_khach_hang,
                        idSanPham = list[i].id_san_pham,
                        idYeuThich = list[i].id_yeu_thich,
                        sanPham = spTam
                    };
                    tam.Add(spyt);
                }
                return Ok(tam);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }  
        
        [HttpPost]
        [ActionName("kiemTra")]
        public IHttpActionResult kiemTraYeuThich (int idSanPham, int IdKhachHang)
        {
            try
            {
                if(IdKhachHang == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(e => e.id_san_pham == idSanPham && e.id_khach_hang == IdKhachHang);

                if (sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                var spYT = new
                {
                    id_yeu_thich = sp.id_yeu_thich,
                    id_san_pham = sp.id_san_pham,
                    id_khach_hang = sp.id_khach_hang
                };
                return Ok(spYT);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //hien thi danh sach san pham yeu thic cua mot khac hang
        [HttpPost]
        [ActionName("DanhSach")]
        public IHttpActionResult getsanPhamPhanTrang([FromBody] PhanTrang phanTrang)
        {
            try
            {
                List<SanPhamYeuThich> list = db.SanPhamYeuThiches.Where(x => x.id_khach_hang == phanTrang.id).ToPagedList(phanTrang.trang, phanTrang.size).ToList();
                List<dynamic> tam = new List<dynamic>();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    SanPham sp = db.SanPhams.FirstOrDefault(e => e.id_san_pham == list[i].id_san_pham);
                    var spTam = new
                    {
                        id_san_pham = sp.id_san_pham,
                        id_danh_muc = sp.id_danh_muc,
                        ten_sp = sp.ten_sp,
                        so_luong = sp.so_luong,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + sp.url_hinh_chinh,
                        mo_ta = sp.mo_ta,
                        phan_tram_km = sp.phan_tram_km,
                        gia_sp = sp.gia_sp,
                        gia_km = sp.gia_km
                    };
                    var spyt = new
                    {
                        idKhachHang = list[i].id_khach_hang,
                        idSanPham = list[i].id_san_pham,
                        idYeuThich = list[i].id_yeu_thich,
                        sanPham = spTam
                    };

                    tam.Add(spyt);
                }
                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [ActionName("themMoi")]
        public IHttpActionResult insertNewSanPhamYeuThich([FromBody] SanPhamYeuThich sanPhamYeuThich)
        {
            try
            {
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_san_pham == sanPhamYeuThich.id_san_pham 
                && x.id_khach_hang == sanPhamYeuThich.id_khach_hang);

                if (sp != null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                db.SanPhamYeuThiches.InsertOnSubmit(sanPhamYeuThich);
                db.SubmitChanges();

                var tam = new
                {
                    id_yeu_thich = sanPhamYeuThich.id_yeu_thich,
                    id_san_pham = sanPhamYeuThich.id_san_pham,
                    id_khach_hang = sanPhamYeuThich.id_khach_hang,
                };

                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("Xoa")]
        public IHttpActionResult deleteSanPhamYeuThich(int id)
        {
            try
            {
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_yeu_thich == id);
                if(sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.SanPhamYeuThiches.DeleteOnSubmit(sp);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
