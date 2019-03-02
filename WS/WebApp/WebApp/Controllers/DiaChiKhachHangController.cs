using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class DiaChiKhachHangController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("DanhSach")]
        public IHttpActionResult danhSach(int id)
        {
            try
            {
                List<DiaChiKhachHang> list = db.DiaChiKhachHangs.Where(e => e.id_khach_hang == id).ToList().Select(e => { e.KhachHang = null;e.QuanHuyen = null; e.TinhThanh = null;e.XaPhuong = null; return e; }).ToList();
                if(list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("taoDiaChiMoi")]
        public IHttpActionResult insert([FromBody] DiaChiKhachHang diaChi)
        {
            try
            {
                db.DiaChiKhachHangs.InsertOnSubmit(diaChi);
                db.SubmitChanges();
                return Ok(diaChi);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [ActionName("update")]
        public IHttpActionResult update([FromBody] DiaChiKhachHang diaChi)
        {
            try
            {
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == diaChi.id);
                if(dc == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                dc.id_quan = diaChi.id_quan;
                dc.id_tinh = diaChi.id_tinh;
                dc.id_xa_phuong = diaChi.id_xa_phuong;
                dc.dia_chi = diaChi.dia_chi;
                dc.loai = diaChi.loai;
                db.SubmitChanges();
                return Ok(dc);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("XoaDiaChi")]
        public IHttpActionResult delete(int id)
        {
            try
            {
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == id);
                if(dc == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.DiaChiKhachHangs.DeleteOnSubmit(dc);
                db.SubmitChanges();
                return Ok(dc);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
