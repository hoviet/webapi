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
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("layMot")]
        public IHttpActionResult laySanPhamYeuThich(int id)//id khach hang
        {
            try
            {
                List<SanPhamYeuThich> list = db.SanPhamYeuThiches.Where(x => x.id_khach_hang == id).ToList();
                List<SanPhamYeuThichHT> tam = new List<SanPhamYeuThichHT>();
                if (list.Count ==0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    SanPhamYeuThichHT spyt = new SanPhamYeuThichHT();
                    spyt.idKhachHang = list[i].id_khach_hang;
                    spyt.idSanPham = list[i].id_san_pham;
                    spyt.idYeuThich = list[i].id_yeu_thich;
                    SanPham sp = db.SanPhams.FirstOrDefault(e => e.id_san_pham == list[i].id_san_pham);
                    SanPham spTam = new SanPham();
                    spTam.id_san_pham = sp.id_san_pham;
                    spTam.id_danh_muc = sp.id_danh_muc;
                    spTam.ten_sp = sp.ten_sp;
                    spTam.so_luong = sp.so_luong;
                    spTam.url_hinh_chinh = sp.url_hinh_chinh;
                    spTam.mo_ta = sp.mo_ta;
                    spTam.phan_tram_km = sp.phan_tram_km;
                    spTam.gia_sp = sp.gia_sp;
                    spTam.gia_km = sp.gia_km;
                    spyt.sanPham = spTam;
                    tam.Add(spyt);
                }
                return Ok(tam);
            } catch (Exception ex)
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
                List<SanPhamYeuThichHT> tam = new List<SanPhamYeuThichHT>();               
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    SanPhamYeuThichHT spyt = new SanPhamYeuThichHT();
                    spyt.idKhachHang = list[i].id_khach_hang;
                    spyt.idSanPham = list[i].id_san_pham;
                    spyt.idYeuThich = list[i].id_yeu_thich;
                    SanPham sp = db.SanPhams.FirstOrDefault(e => e.id_san_pham == list[i].id_san_pham);
                    SanPham spTam = new SanPham();
                    spTam.id_san_pham = sp.id_san_pham;
                    spTam.id_danh_muc = sp.id_danh_muc;
                    spTam.ten_sp = sp.ten_sp;
                    spTam.so_luong = sp.so_luong;
                    spTam.url_hinh_chinh = sp.url_hinh_chinh;
                    spTam.mo_ta = sp.mo_ta;
                    spTam.phan_tram_km = sp.phan_tram_km;
                    spTam.gia_sp = sp.gia_sp;
                    spTam.gia_km = sp.gia_km;
                    spyt.sanPham = spTam;
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
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_san_pham == sanPhamYeuThich.id_san_pham && x.id_khach_hang == sanPhamYeuThich.id_khach_hang);
                if (sp != null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                db.SanPhamYeuThiches.InsertOnSubmit(sanPhamYeuThich);
                db.SubmitChanges();
                SanPhamYeuThich tam = new SanPhamYeuThich();
                tam.id_yeu_thich = sanPhamYeuThich.id_yeu_thich;
                tam.id_san_pham = sanPhamYeuThich.id_san_pham;
                tam.id_khach_hang = sanPhamYeuThich.id_khach_hang;
                
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
