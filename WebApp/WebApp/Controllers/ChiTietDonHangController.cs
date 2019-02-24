using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class ChiTietDonHangController : ApiController
    {
        [HttpGet]
        public List<ChiTietDonHang> getChiTietDonHang()
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

            return db.ChiTietDonHangs.ToList();
        }
        [HttpPost]
        public IHttpActionResult insertNewChiTietDonHang([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

                db.ChiTietDonHangs.InsertOnSubmit(chiTietDonHang);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public IHttpActionResult updateChiTietDonHang([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                ChiTietDonHang ct = db.ChiTietDonHangs.FirstOrDefault(x => x.id_don_hang == chiTietDonHang.id_don_hang && x.id_san_pham == chiTietDonHang.id_san_pham);
                if(ct == null)
                {
                    return NotFound();
                }
                if(chiTietDonHang.so_luong != 0)
                {
                    ct.so_luong = chiTietDonHang.so_luong;
                }
                if(chiTietDonHang.gia_km != 0)
                {
                    ct.gia_km = chiTietDonHang.gia_km;
                }
                if(chiTietDonHang.tong_tien != 0)
                {
                    ct.tong_tien = chiTietDonHang.tong_tien;
                }
                if(chiTietDonHang.thoi_gian_lap.Equals("0001 - 01 - 01T00: 00:00"))
                {
                    ct.thoi_gian_lap = chiTietDonHang.thoi_gian_lap;
                }
                
                db.SubmitChanges();
                return Ok(ct);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IHttpActionResult deleteChiTietSanPham([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                ChiTietDonHang ct = db.ChiTietDonHangs.FirstOrDefault(x => x.id_don_hang == chiTietDonHang.id_don_hang && x.id_san_pham == chiTietDonHang.id_san_pham);
                if(ct == null)
                {
                    return NotFound();
                }

                db.ChiTietDonHangs.DeleteOnSubmit(ct);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
