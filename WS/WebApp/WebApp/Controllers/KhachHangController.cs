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
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("DangNhap")]
        public IHttpActionResult dangNhap(String ten, String matKhau)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.tai_khoan == ten && x.mat_khau == matKhau);
                if(kh == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                kh.mat_khau = null;
                return Ok(kh);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getMot")] // lay thong tin khach hang, truyen vao id_khach_hang
        public IHttpActionResult getMotKhachHang(int id)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == id);
                if(kh == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                kh.DiaChiKhachHangs = null;
                kh.DonDatHangs = null;
                kh.SanPhamYeuThiches = null;
                return Ok(kh);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //them
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult InsertNewKhachHang([FromBody] KhachHang khachhang)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.tai_khoan == khachhang.tai_khoan|| x.so_dt == khachhang.so_dt||x.email == khachhang.email);
                if(kh != null)
                {
                    return BadRequest("Đã tồn tại");
                }
                db.KhachHangs.InsertOnSubmit(khachhang);
                db.SubmitChanges();
                return Ok(khachhang);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //sua
        [HttpPost]
        [ActionName("update")]
        public IHttpActionResult updateKhachHang([FromBody] KhachHang khachhang)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == khachhang.id_khach_hang);


                if (kh== null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
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
        [ActionName("delete")]
        public IHttpActionResult DeleteKhachHang (int id)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == id);
                if (kh == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
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
