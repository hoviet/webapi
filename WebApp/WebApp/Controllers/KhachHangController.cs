using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class KhachHangController : ApiController
    {
        
        [HttpGet]
        public List<KhachHang> getKhachHangLists()
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
            return db.KhachHangs.ToList();
        }

        [HttpGet]
        public KhachHang getKhachHang(int id)
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

            return db.KhachHangs.FirstOrDefault(X => X.id_khach_hang == id);
        }
    
        //them
        [HttpPost]
        public IHttpActionResult InsertNewKhachHang([FromBody] KhachHang khachhang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

               
               // cần thay đôi gì thi thay doi
               // k cần thì save luong

                db.KhachHangs.InsertOnSubmit(khachhang);
                db.SubmitChanges();
                //ok la ham thanh cong
                // khi muon tra ve cai gì thì truyền trong ok 
                return Ok(khachhang);
            }
            catch(Exception ex)
            {
                // phải tra loi để còn mò dc lỗi
                // k là sml
                return BadRequest(ex.Message);
            }
        }
        //sua
        [HttpPut]
        public IHttpActionResult updateKhachHang([FromBody] KhachHang khachhang)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == khachhang.id_khach_hang);


                if (kh== null)
                {
                    return NotFound();
                }
                if(khachhang.tai_khoan != null)
                {
                    kh.tai_khoan = khachhang.tai_khoan;
                }
                if (khachhang.mat_khau != null)
                {
                    kh.mat_khau = khachhang.mat_khau;
                }
                if(khachhang.ten_nguoi_dung != null)
                {
                    kh.ten_nguoi_dung = khachhang.ten_nguoi_dung;
                }
                if(khachhang.so_dt != null)
                {
                    kh.so_dt = khachhang.so_dt;
                }
                if(khachhang.email != null)
                {
                    kh.email = khachhang.email;
                }
                if(khachhang.ngay_sinh != null)
                {
                    kh.ngay_sinh = khachhang.ngay_sinh;
                }

                db.SubmitChanges();
                return Ok(kh);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteKhachHang (int id)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == id);
                if (kh == null)
                {
                    return NotFound();
                }
                db.KhachHangs.DeleteOnSubmit(kh);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
