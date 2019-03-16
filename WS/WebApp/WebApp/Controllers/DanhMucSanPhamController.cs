using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class DanhMucSanPhamController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("danhSach")]
        public IHttpActionResult getSanPhamList()
        {
            try
            {
                List<DanhMucSanPham> listTam = db.DanhMucSanPhams.ToList().Select(e =>
                {
                    e.SanPhams = null;
                    return e;
                }).ToList();
                if (listTam.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(listTam);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("layMot")]
        public IHttpActionResult MotLoaiSanPham(int id)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == id);
                if(dmsp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                var tam = new
                {
                    idDanhMuc = dmsp.id_danh_muc,
                    tenDanhMuc = dmsp.ten_danh_muc,
                    urlHinh = dmsp.url_hinh
                };
                return Ok(dmsp);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        // url Hinh = base64
        [HttpPost]
        [ActionName("them")]    
        public IHttpActionResult inserNewDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPham)
        {
            try
            {
                DanhMucSanPham dsp = db.DanhMucSanPhams.FirstOrDefault(e => e.ten_danh_muc.Equals(danhMucSanPham.ten_danh_muc));
                if(dsp != null)
                {
                    var dm = new
                    {
                        idDanhMuc = dsp.id_danh_muc,
                        tenDanhMuc = dsp.ten_danh_muc,
                        urlHinh = dsp.url_hinh
                    };
                    return Ok(dm);
                }
                string base64 = danhMucSanPham.url_hinh;
                danhMucSanPham.url_hinh = "";

                db.DanhMucSanPhams.InsertOnSubmit(danhMucSanPham);
                db.SubmitChanges();

                byte[] imageBytes = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string fileName = "sanpham_" + danhMucSanPham.id_danh_muc + ".png";
                image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/DanhMucSanPham"), fileName));

                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(e => e.id_danh_muc == danhMucSanPham.id_danh_muc);
                dmsp.url_hinh = "~/hinh/DanhMucSanPham" + fileName;

                db.SubmitChanges();

                var tam = new
                {
                    idDanhMuc = dmsp.id_danh_muc,
                    tenDanhMuc = dmsp.ten_danh_muc,
                    urlHinh = dmsp.url_hinh
                };
                return Ok(tam);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [ActionName("sua")]
        public IHttpActionResult updaeDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPham)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == danhMucSanPham.id_danh_muc);
                if(dmsp == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                if(danhMucSanPham.ten_danh_muc != null)
                {
                    dmsp.ten_danh_muc = danhMucSanPham.ten_danh_muc;
                }
                //if(danhMucSanPham.url_hinh != null)
                //{
                //    dmsp.url_hinh = danhMucSanPham.url_hinh;
                //}

                db.SubmitChanges();
                var tam = new
                {
                    idDanhMuc = dmsp.id_danh_muc,
                    tenDanhMuc = dmsp.ten_danh_muc,
                    urlHinh = dmsp.url_hinh
                };
                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("xoa")]
        public IHttpActionResult deleteDanhMucSanPham(int id)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == id);

                if(dmsp == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }

                db.DanhMucSanPhams.DeleteOnSubmit(dmsp);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
