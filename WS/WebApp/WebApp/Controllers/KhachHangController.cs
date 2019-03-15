using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class KhachHangController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("DangNhap")]
        public IHttpActionResult dangNhap(string ten, string matKhau)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.tai_khoan == ten && x.mat_khau == matKhau);
                if (kh == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                kh.mat_khau = null;
                DateTime ngay = (DateTime)kh.ngay_sinh;
                var tam = new
                {
                    id_khach_hang = kh.id_khach_hang,
                    tai_khoan = kh.tai_khoan,
                    ten_nguoi_dung = kh.ten_nguoi_dung,
                    ngay_sinh = ngay.ToShortDateString(),
                    so_dt = kh.so_dt,
                    gioi_tinh = kh.gioi_tinh,
                    t_dang_ky = kh.t_dang_ky.ToShortDateString(),
                    email = kh.email,
                    url_hinh = kh.url_hinh
                };
                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [ActionName("DangNhapFCM")]
        public IHttpActionResult DangNhamFCM([FromBody] FCMDangNhap fCMDangNhap)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.tai_khoan == fCMDangNhap.taiKhoan && x.mat_khau == fCMDangNhap.matKhau);
                if (kh == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                DateTime ngay = (DateTime)kh.ngay_sinh;
                var tam = new
                {
                    id_khach_hang = kh.id_khach_hang,
                    tai_khoan = kh.tai_khoan,
                    ten_nguoi_dung = kh.ten_nguoi_dung,
                    ngay_sinh = ngay.ToShortDateString(),
                    so_dt = kh.so_dt,
                    gioi_tinh = kh.gioi_tinh,
                    t_dang_ky = kh.t_dang_ky.ToShortDateString(),
                    email = kh.email,
                    url_hinh = kh.url_hinh
                };
                FCM fcm = db.FCMs.FirstOrDefault(e => e.token.Equals(fCMDangNhap.token) && e.device.Equals(fCMDangNhap.device));
                if(fcm == null)
                {
                    FCM fc= new FCM();

                    fc.khach_hang = tam.id_khach_hang;
                    fc.token = fCMDangNhap.token;
                    fc.device = fCMDangNhap.device;

                    db.FCMs.InsertOnSubmit(fc);
                    db.SubmitChanges();
                    return Ok(tam);
                }

                fcm.khach_hang = tam.id_khach_hang;

                db.SubmitChanges();
                return Ok(tam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        [HttpGet]
        [ActionName("DangXuatFCM")]
        public IHttpActionResult DangXuatFCM(int idKhachHang)
        {
            try
            {         
                FCM fcm = db.FCMs.FirstOrDefault(e => e.khach_hang == idKhachHang);
                if (fcm == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                fcm.khach_hang = 0;
                db.SubmitChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }       
        //
        [HttpGet]
        [ActionName("danhSach")]
        public IHttpActionResult dsKhachHang()
        {
            try
            {
                List<KhachHang> list = db.KhachHangs.ToList().Select(e =>
                {
                    e.DonDatHangs = null;
                    e.DiaChiKhachHangs = null;
                    e.SanPhamYeuThiches = null;
                    return e;
                }).ToList();
                if(list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                return Ok(list);
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
                DateTime ngay = (DateTime)kh.ngay_sinh;
                var tam = new
                {
                    id_khach_hang = kh.id_khach_hang,
                    tai_khoan = kh.tai_khoan,
                    ten_nguoi_dung = kh.ten_nguoi_dung,
                    ngay_sinh = ngay.ToShortDateString(),
                    so_dt = kh.so_dt,
                    gioi_tinh = kh.gioi_tinh,
                    t_dang_ky = kh.t_dang_ky.ToShortDateString(),
                    email = kh.email,
                    url_hinh = kh.url_hinh
                };
                return Ok(tam);
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
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.KhachHangs.InsertOnSubmit(khachhang);
                db.SubmitChanges();

                DateTime ngay = (DateTime)khachhang.ngay_sinh;
                var tam = new
                {
                    id_khach_hang = khachhang.id_khach_hang,
                    tai_khoan = khachhang.tai_khoan,
                    ten_nguoi_dung = khachhang.ten_nguoi_dung,
                    ngay_sinh = ngay.ToShortDateString(),
                    so_dt = khachhang.so_dt,
                    gioi_tinh = khachhang.gioi_tinh,
                    t_dang_ky = khachhang.t_dang_ky.ToShortDateString(),
                    email = khachhang.email,
                    url_hinh = khachhang.url_hinh
                };
                return Ok(tam);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //doi mat khau
        [HttpPost]
        [ActionName("doiMatKhau")]
        public IHttpActionResult doiMatKhau([FromBody] DoiMatKhau doMK)
        {
            try
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(e => e.id_khach_hang==doMK.idKhachHang && e.mat_khau.Equals(doMK.matKhauCu));
                if(kh == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                kh.mat_khau = doMK.matKhauMoi;
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
                
                if(khachhang.ten_nguoi_dung != null)
                {
                    kh.ten_nguoi_dung = khachhang.ten_nguoi_dung;
                }                             
                if(khachhang.ngay_sinh != null)
                {
                    kh.ngay_sinh = khachhang.ngay_sinh;
                } 
                if(khachhang.gioi_tinh != null)
                {
                    kh.gioi_tinh = khachhang.gioi_tinh;
                }

                db.SubmitChanges();
                DateTime ngay = (DateTime)kh.ngay_sinh;
                var tam = new
                {
                    id_khach_hang = kh.id_khach_hang,
                    tai_khoan = kh.tai_khoan,
                    ten_nguoi_dung = kh.ten_nguoi_dung,
                    ngay_sinh = ngay.ToShortDateString(),
                    so_dt = kh.so_dt,
                    gioi_tinh = kh.gioi_tinh,
                    t_dang_ky = kh.t_dang_ky.ToShortDateString(),
                    email = kh.email,
                    url_hinh = kh.url_hinh
                };              

                return Ok(tam);
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
        [HttpPost]
        [ActionName("themhinh")]
        public IHttpActionResult hinhAnh([FromBody] KhachHang khachHang)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(khachHang.url_hinh);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string fileName = khachHang.id_khach_hang + ".png";
                image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/KhachHang"), fileName));

                KhachHang kh = db.KhachHangs.FirstOrDefault(e => e.id_khach_hang == khachHang.id_khach_hang);
                if(kh== null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                kh.url_hinh = "~/hinh/KhachHang" + fileName;

                db.SubmitChanges();
                return Ok();

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
