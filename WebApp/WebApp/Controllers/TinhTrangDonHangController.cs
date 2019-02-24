using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class TinhTrangDonHangController : ApiController
    {
        [HttpGet]
        public List<TinhTrangDonHang> getTinhTrangDonHangList()
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
            return db.TinhTrangDonHangs.ToList();
        }

        [HttpGet]
        public TinhTrangDonHang getTinhTrangDonHang(int id)
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
            return db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == id);
        }

        [HttpPost]
        public IHttpActionResult insertNewTinhTrangDonHang([FromBody] TinhTrangDonHang tinhTrangDonHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

                db.TinhTrangDonHangs.InsertOnSubmit(tinhTrangDonHang);
                db.SubmitChanges();
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult updateTinhTrangDonHang([FromBody] TinhTrangDonHang tinhTrangDonHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == tinhTrangDonHang.id_tinh_trang);

                if(tr == null)
                {
                    return NotFound();
                }
                if(tinhTrangDonHang.tinh_trang_don_hang != null)
                {
                    tr.tinh_trang_don_hang = tinhTrangDonHang.tinh_trang_don_hang;
                }
                if(tinhTrangDonHang.ghi_chu != null)
                {
                    tr.ghi_chu = tinhTrangDonHang.ghi_chu;
                }

                db.SubmitChanges();
                return Ok(tr);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IHttpActionResult deleteTinhTrangDonHang([FromBody] TinhTrangDonHang tinhTrangDonHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == tinhTrangDonHang.id_tinh_trang);

                if(tr == null)
                {
                    return NotFound();
                }
                db.TinhTrangDonHangs.DeleteOnSubmit(tr);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
