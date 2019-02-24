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
        [ActionName("getYeuThich")]
        public IHttpActionResult laySanPhamYeuThich(int id)
        {
            try
            {
                List<SanPhamYeuThich> list = db.SanPhamYeuThiches.Where(x => x.id_khach_hang == id).ToList();
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].KhachHang = null;
                    list[i].SanPham.DanhMucSanPham = null;
                }
                return Ok(list);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getYeuThichPhanTrang")]
        public IHttpActionResult getsanPhamPhanTrang([FromBody] PhanTrang phanTrang)
        {
            try
            {
                List<SanPhamYeuThich> list = db.SanPhamYeuThiches.Where(x => x.id_khach_hang == phanTrang.id).ToPagedList(phanTrang.trang, phanTrang.size).ToList();
                if(list == null)
                {
                    return NotFound();
                }

                for (int i = 0; i < list.Count; i++)
                {
                    list[i].KhachHang = null;
                    list[i].SanPham.DanhMucSanPham = null;
                }
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult insertNewSanPhamYeuThich([FromBody] SanPhamYeuThich sanPhamYeuThich)
        {
            try
            {
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_san_pham == sanPhamYeuThich.id_san_pham && x.id_khach_hang == sanPhamYeuThich.id_khach_hang);
                if(sp!= null)
                {
                    return Ok(false);
                }
                db.SanPhamYeuThiches.InsertOnSubmit(sanPhamYeuThich);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult deleteSanPhamYeuThich(int id)
        {
            try
            {
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_yeu_thich == id);
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
