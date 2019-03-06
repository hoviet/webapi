using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PagedList;
using WebApp.Models;

namespace WebApp.Controllers
{
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
                List<SanPhamYeuThich> tam = new List<SanPhamYeuThich>();
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count; i++)
                {
                    SanPhamYeuThich sp = new SanPhamYeuThich();
                    sp.id_khach_hang = list[i].id_khach_hang;
                    sp.id_san_pham = list[i].id_san_pham;
                    sp.id_yeu_thich = list[i].id_yeu_thich;
                    tam.Add(sp);
                }
                return Ok(tam);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //hien thi danh sach san pham yeu thic cau mot khac hang
        [HttpPost]
        [ActionName("DanhSach")]
        public IHttpActionResult getsanPhamPhanTrang([FromBody] PhanTrang phanTrang)
        {
            try
            {
                List<SanPhamYeuThich> list = db.SanPhamYeuThiches.Where(x => x.id_khach_hang == phanTrang.id).ToPagedList(phanTrang.trang, phanTrang.size).ToList();
                List<SanPham> lsp = new List<SanPham>();
                 if(list == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    SanPham sp = db.SanPhams.FirstOrDefault(e => e.id_san_pham == list[i].id_san_pham);
                    sp.SanPhamYeuThiches = null;
                    sp.DanhMucSanPham = null;
                    sp.ChiTietDonHangs = null;
                    lsp.Add(sp);
                }
                return Ok(lsp);
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
                if(sp!= null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.SanPhamYeuThiches.InsertOnSubmit(sanPhamYeuThich);
                db.SubmitChanges();
                return Ok(sanPhamYeuThich);
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
