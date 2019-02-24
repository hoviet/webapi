using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class DonDatHangController : ApiController
    {
        [HttpGet]
        public DonDatHang getDonDatHang(int id)
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
            return db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
        }

        [HttpPost]
        public IHttpActionResult insertDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

                db.DonDatHangs.InsertOnSubmit(donDatHang);
                db.SubmitChanges();
                return Ok(true);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult updateDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                DonDatHang ddh = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == donDatHang.id_don_hang);

                if(ddh == null)
                {
                    return NotFound();
                }
                if(donDatHang.id_tinh_trang != 0)
                {
                    ddh.id_tinh_trang = donDatHang.id_tinh_trang;
                }
                if(donDatHang.ngay_lap.Equals("0001 - 01 - 01T00: 00:00"))
                {
                    ddh.ngay_lap = donDatHang.ngay_lap;
                }
                if(donDatHang.tong_tien != 0)
                {
                    ddh.tong_tien = donDatHang.tong_tien;
                }
                if(donDatHang.so_dt_nguoi_nhan != 0)
                {
                    ddh.so_dt_nguoi_nhan = donDatHang.so_dt_nguoi_nhan;
                }
                if(donDatHang.noi_nhan != null)
                {
                    ddh.noi_nhan = donDatHang.noi_nhan;
                }
                if(donDatHang.ghi_chu != null)
                {
                    ddh.ghi_chu = donDatHang.ghi_chu;
                }

                db.SubmitChanges();
                return Ok(ddh);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IHttpActionResult deleteDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                DonDatHang ddh = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == donDatHang.id_don_hang);
                if(ddh == null)
                {
                    return NotFound();
                }
                db.DonDatHangs.DeleteOnSubmit(ddh);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
