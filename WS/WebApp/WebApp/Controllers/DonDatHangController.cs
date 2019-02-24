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
    public class DonDatHangController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("getList")]
       public IHttpActionResult getListHoaDon([FromBody] PhanTrang phanTrang)
        {
            try
            {
                List<DonDatHang> list = db.DonDatHangs.Where(x => x.id_khach_hang == phanTrang.id).ToPagedList(phanTrang.trang, phanTrang.size).ToList();
                if(list == null)
                {
                    return NotFound();
                }
                for(int i = 0; i <list.Count; i++)
                {
                    list[i].KhachHang.SanPhamYeuThiches = null;
                    list[i].ChiTietDonHangs = null;
                }
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getMotHoaDon")]
        public IHttpActionResult getMotHoaDon(int id)
        {
            try
            {
                DonDatHang hd = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
                HoaDon hoaDonTam = new HoaDon();
                if(hd == null)
                {
                    return NotFound();
                }
                hoaDonTam.id = hd.id_don_hang;
                hoaDonTam.idKhachHang = hd.id_khach_hang;
                hoaDonTam.idTinhTrang = hd.id_tinh_trang;
                hoaDonTam.ngayLap = hd.ngay_lap;
                hoaDonTam.noiNhan = hd.noi_nhan;
                hoaDonTam.soDT = hd.so_dt_nguoi_nhan;
                hoaDonTam.tongGia =(float) hd.tong_tien;
                hoaDonTam.ghiChu = hd.ghi_chu;
                return Ok(hoaDonTam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult insertDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
                db.DonDatHangs.InsertOnSubmit(donDatHang);
                db.SubmitChanges();
                return Ok(true);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult updateDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
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
        [ActionName("delete")]
        public IHttpActionResult deleteDonDatHang(int id)
        {
            try
            {
                DonDatHang ddh = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
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
