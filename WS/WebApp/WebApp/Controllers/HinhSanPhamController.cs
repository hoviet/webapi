using PagedList;
using System;
using System.Collections.Generic;
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
    public class HinhSanPhamController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult danhSach(int idSanPham)
        {
            try
            {
                List<HinhSP> hsp = db.HinhSPs.Where(e => e.id_sp == idSanPham).ToList();
                if (hsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }              
                List<dynamic> list = new List<dynamic>();
                for (int i = 0; i < hsp.Count; i++)
                {
                    var tam = new
                    {
                        idHinh = hsp[i].id_hinh,
                        idSanPham = hsp[i].id_sp,
                        urlHinh = hsp[i].url_hinh
                    };
                    list.Add(tam);
                }
                return Ok(list);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult danhSach()
        {
            try
            {
                List<HinhSP> hsp = db.HinhSPs.ToList();
                if(hsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                int a = hsp.Count;

                hsp.ToPagedList(1, 10).ToList();
                List<dynamic> list = new List<dynamic>();
                for(int i = 0; i < hsp.Count; i++)
                {
                    var tam = new
                    {
                        idHinh = hsp[i].id_hinh,
                        idSanPham = hsp[i].id_sp,
                        urlHinh = hsp[i].url_hinh
                    };
                    list.Add(tam);
                }
                var obj = new
                {
                    count = a,
                    list = list
                };
                return Ok(obj);
               
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult danhSach(int page, int size)
        {
            try
            {
                List<HinhSP> hsp = db.HinhSPs.ToList().ToPagedList(page, size).ToList();
                if (hsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
               
                List<dynamic> list = new List<dynamic>();
                for (int i = 0; i < hsp.Count; i++)
                {
                    var tam = new
                    {
                        idHinh = hsp[i].id_hinh,
                        idSanPham = hsp[i].id_sp,
                        urlHinh = hsp[i].url_hinh
                    };
                    list.Add(tam);
                }

                return Ok(list);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("them")]
        public IHttpActionResult themHinh([FromBody] hinhAnh hinh)
        {
            try
            {
                if(db.SanPhams.FirstOrDefault(e=>e.id_san_pham == hinh.idSanpham) == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }

                for(int i = 0; i < hinh.base64.Count; i++)
                {
                    HinhSP hinhTam = new HinhSP();
                    hinhTam.id_sp = hinh.idSanpham;

                    byte[] imageBytes = Convert.FromBase64String(hinh.base64[i]);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    string fileName = hinh.idSanpham +"stt"+ i + ".png";
                    image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/SanPham"), fileName));

                    hinhTam.url_hinh = "~/hinh/SanPham/" + fileName;

                    db.HinhSPs.InsertOnSubmit(hinhTam);
                    db.SubmitChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("xoa")]
        public IHttpActionResult xoa(int idHinh)
        {
            try
            {
                HinhSP hinh = db.HinhSPs.FirstOrDefault(e => e.id_hinh == idHinh);
                if(hinh == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                db.HinhSPs.DeleteOnSubmit(hinh);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
