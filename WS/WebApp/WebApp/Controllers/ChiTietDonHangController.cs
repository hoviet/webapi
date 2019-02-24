using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ChiTietDonHangController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("getListMotDon")]
        public IHttpActionResult getChiTietDonHang([FromBody]ChiTietDonHang chiTietDonHang)
        {
            try
            {
                List<ChiTietDonHang> list = db.ChiTietDonHangs.Where(x => x.id_don_hang == chiTietDonHang.id_don_hang).ToList();
                if(list == null)
                {
                    return NotFound();
                }
                for(int i = 0; i < list.Count; i++)
                {
                    list[i].SanPham.SanPhamYeuThiches = null;
                    list[i].SanPham.DanhMucSanPham = null;
                    list[i].DonDatHang = null;
                }
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult insertNewChiTietDonHang([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
                db.ChiTietDonHangs.InsertOnSubmit(chiTietDonHang);
                db.SubmitChanges();
                return Ok(chiTietDonHang);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult updateChiTietDonHang([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
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
        [ActionName("delete")]
        public IHttpActionResult deleteChiTietSanPham([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
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
