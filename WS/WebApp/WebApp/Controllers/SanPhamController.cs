using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PagedList;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    [RoutePrefix("")]
    public class SanPhamController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("getMotSanPham")]
        public IHttpActionResult getSanPham(int id)
        {
            try
            {
                SanPham sp = db.SanPhams.FirstOrDefault(x => x.id_san_pham == id);

                if (sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                var tam = new
                {
                    id_san_pham = sp.id_san_pham,
                    id_danh_muc = sp.id_danh_muc,
                    ten_sp = sp.ten_sp,
                    so_luong = sp.so_luong,
                    url_hinh_chinh = "http://www.3anhem.somee.com" + sp.url_hinh_chinh,
                    mo_ta = sp.mo_ta,
                    phan_tram_km = sp.phan_tram_km,
                    gia_sp = sp.gia_sp,
                    gia_km = sp.gia_km,
                };
                return Ok(tam);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //lay mot san pham co them thuoc tinh yeu thi hay ko
        [HttpGet]
        [ActionName("MotSanPham")]
        public IHttpActionResult SanPham(int idSanPham,int idKhachHang)
        {
            try
            {
                SanPham sp = db.SanPhams.FirstOrDefault(x => x.id_san_pham == idSanPham);
                SanPham tam = new SanPham();
                SanPhamYeuThich spyt = db.SanPhamYeuThiches.FirstOrDefault(e => e.id_san_pham == idSanPham && e.id_khach_hang == idKhachHang);
                int idlove = 0;
                if (spyt != null)
                {
                    idlove = spyt.id_yeu_thich;
                }
                if (sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                tam.id_san_pham = sp.id_san_pham;
                tam.id_danh_muc = sp.id_danh_muc;
                tam.ten_sp = sp.ten_sp;
                tam.so_luong = sp.so_luong;
                tam.url_hinh_chinh = "http://www.3anhem.somee.com" + sp.url_hinh_chinh;
                tam.mo_ta = sp.mo_ta;
                tam.phan_tram_km = sp.phan_tram_km;
                tam.gia_sp = sp.gia_sp;
                tam.gia_km = sp.gia_km;
                var sanPham = new
                {
                    SanPham = tam,
                    idYeuThich = idlove
                };
                return Ok(sanPham);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getTheoDanhMuc")]
        public IHttpActionResult getListSanPham(int id)
        {
            try
            {
                List<SanPham> lsp = db.SanPhams.Where(x => x.id_danh_muc == id).ToList();
                if (lsp.Count==0) 
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> obj = new List<dynamic>();
                for(int i = 0; i <lsp.Count; i++)
                {
                    var tam = new
                    {
                        id_san_pham = lsp[i].id_san_pham,
                        id_danh_muc = lsp[i].id_danh_muc,
                        ten_sp = lsp[i].ten_sp,
                        gia_sp = lsp[i].gia_sp,
                        phan_tram_km = lsp[i].phan_tram_km,
                        gia_km = lsp[i].gia_km,
                        so_luong = lsp[i].so_luong,
                        mo_ta = lsp[i].mo_ta,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + lsp[i].url_hinh_chinh
                    };
                    obj.Add(tam);
                }
                return Ok(obj);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("TheoDanhMuc")]
        public IHttpActionResult getListSanPham(int id, int locTheo)
        {
            try
            {
                List<SanPham> lsp = new List<SanPham>();
                if (locTheo == 0) //gia tu thap den cao
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == id).OrderBy(e => e.gia_km).ToList();
                }
                if (locTheo == 1) //gia tu cao den thap
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == id).OrderByDescending(e => e.gia_km).ToList();
                }
                if (locTheo == 2) //phan tram khuyen mai cao nhat
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == id).OrderByDescending(e => e.phan_tram_km).ToList();
                }
                if (lsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> obj = new List<dynamic>();
                for (int i = 0; i < lsp.Count; i++)
                {
                    var tam = new {
                        id_san_pham = lsp[i].id_san_pham,
                        id_danh_muc = lsp[i].id_danh_muc,
                        ten_sp = lsp[i].ten_sp,
                        gia_sp = lsp[i].gia_sp,
                        phan_tram_km = lsp[i].phan_tram_km,
                        gia_km = lsp[i].gia_km,
                        so_luong = lsp[i].so_luong,
                        mo_ta = lsp[i].mo_ta,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + lsp[i].url_hinh_chinh
                    };
                    obj.Add(tam);
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [ActionName("getPhanTrang")]
        public IHttpActionResult getSanPham10([FromBody] PhanTrang pt)// phanTrang{id, Trang, Size}
        {
            try
            {
                List<SanPham> lsp = new List<SanPham>();
                if (pt.locTheo == 0) //gia tu thap den cao
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == pt.id).OrderBy(e => e.gia_km).ToList().ToPagedList(pt.trang, pt.size).ToList();
                }
                if (pt.locTheo == 1) //gia tu cao den thap
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == pt.id).OrderByDescending(e => e.gia_km).ToList().ToPagedList(pt.trang, pt.size).ToList();
                }
                if (pt.locTheo == 2) //phan tram khuyen mai cao nhat
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == pt.id).OrderByDescending(e => e.phan_tram_km).ToList().ToPagedList(pt.trang, pt.size).ToList();
                }
                if (lsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> obj = new List<dynamic>();
                for (int i = 0; i < lsp.Count; i++)
                {
                    var tam = new
                    {
                        id_san_pham = lsp[i].id_san_pham,
                        id_danh_muc = lsp[i].id_danh_muc,
                        ten_sp = lsp[i].ten_sp,
                        gia_sp = lsp[i].gia_sp,
                        phan_tram_km = lsp[i].phan_tram_km,
                        gia_km = lsp[i].gia_km,
                        so_luong = lsp[i].so_luong,
                        mo_ta = lsp[i].mo_ta,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + lsp[i].url_hinh_chinh
                    };
                    obj.Add(tam);
                }
                return Ok(obj);
            }            
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("DSPhanTrang")]
        public IHttpActionResult SanPham10([FromBody] PhanTrang pt)// phanTrang{id, Trang, Size}
        {
            try
            {
                List<SanPham> lsp = new List<SanPham>();
                if (pt.locTheo == 0) //gia tu thap den cao
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == pt.id).OrderBy(e => e.gia_km).ToList().ToPagedList(pt.trang,pt.size).ToList();
                }
                if (pt.locTheo == 1) //gia tu cao den thap
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == pt.id).OrderByDescending(e => e.gia_km).ToList().ToPagedList(pt.trang, pt.size).ToList();
                }
                if (pt.locTheo == 2) //phan tram khuyen mai cao nhat
                {
                    lsp = db.SanPhams.Where(x => x.id_danh_muc == pt.id).OrderByDescending(e => e.phan_tram_km).ToList().ToPagedList(pt.trang, pt.size).ToList();
                }
                if (lsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> obj = new List<dynamic>();
                for (int i = 0; i < lsp.Count; i++)
                {
                    var tam = new
                    {
                        id_san_pham = lsp[i].id_san_pham,
                        id_danh_muc = lsp[i].id_danh_muc,
                        ten_sp = lsp[i].ten_sp,
                        gia_sp = lsp[i].gia_sp,
                        phan_tram_km = lsp[i].phan_tram_km,
                        gia_km = lsp[i].gia_km,
                        so_luong = lsp[i].so_luong,
                        mo_ta = lsp[i].mo_ta,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + lsp[i].url_hinh_chinh
                    };
                    obj.Add(tam);
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("DanhSach")]
        public IHttpActionResult layHet()
        {
            try
            {
                List<SanPham> listt = db.SanPhams.ToList();
                if (listt.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                int a = listt.Count;

                List<SanPham> list =listt.ToPagedList(1, 10).ToList();

                List<dynamic> lds = new List<dynamic>();
                for (int i = 0; i < list.Count; i++)
                {
                    var sp = new
                    {
                        id_san_pham = list[i].id_san_pham,
                        id_danh_muc = list[i].id_danh_muc,
                        ten_sp = list[i].ten_sp,
                        so_luong = list[i].so_luong,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + list[i].url_hinh_chinh,
                        mo_ta = list[i].mo_ta,
                        phan_tram_km = list[i].phan_tram_km,
                        gia_sp = list[i].gia_sp,
                        gia_km = list[i].gia_km
                    };
                    lds.Add(sp);
                }
                var tam = new
                {
                    count = a,
                    ltam = lds
                };
                return Ok(tam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("DanhSach")]
        public IHttpActionResult Phantrang(int page, int size)
        {
            try
            {

                List<SanPham> listt = db.SanPhams.ToList();
                if (listt.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                int a = listt.Count;

                List<SanPham> list = listt.ToPagedList(page, size).ToList();

                list.ToPagedList(1, 10).ToList();

                List<dynamic> lds = new List<dynamic>();
                for (int i = 0; i < list.Count; i++)
                {
                    var sp = new
                    {
                        id_san_pham = list[i].id_san_pham,
                        id_danh_muc = list[i].id_danh_muc,
                        ten_sp = list[i].ten_sp,
                        so_luong = list[i].so_luong,
                        url_hinh_chinh = "http://www.3anhem.somee.com" + list[i].url_hinh_chinh,
                        mo_ta = list[i].mo_ta,
                        phan_tram_km = list[i].phan_tram_km,
                        gia_sp = list[i].gia_sp,
                        gia_km = list[i].gia_km
                    };
                    lds.Add(sp);
                }
                var tam = new
                {
                    count = a,
                    ltam = lds
                };
                return Ok(tam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult insertNewSanPham([FromBody] SanPham sanPham)
        {
            try
            {
                // kiểm tra thông tin đầu vào
                if (string.IsNullOrEmpty(sanPham.mo_ta) ||
                    string.IsNullOrEmpty(sanPham.ten_sp) ||
                    string.IsNullOrEmpty(sanPham.url_hinh_chinh) ||
                    sanPham.id_danh_muc == 0 ||
                    sanPham.gia_sp == 0
                )
                {
                    return Ok(new ResponseData
                    {
                        ResponseCode = 1,
                        ResponseMessage = "Thông tin nhập chưa đúng"
                    });
                }
                // copy data hình ảnh
                string hinh = sanPham.url_hinh_chinh;

                // insert sản phẩm vào db
                sanPham.url_hinh_chinh = "";
                db.SanPhams.InsertOnSubmit(sanPham);
                db.SubmitChanges();

                // save hình vào server
                byte[] imageBytes = Convert.FromBase64String(hinh);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string fileName = "san_pham_" + sanPham.id_san_pham + ".png";
                image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/SanPham"), fileName));

                sanPham.url_hinh_chinh = "/hinh/SanPham/" + fileName;
                db.SubmitChanges();

                return Ok(new ResponseData
                {
                    ResponseCode = 2,
                    ResponseMessage = "Thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseData
                {
                    ResponseCode = 1,
                    ResponseMessage = ex.Message
                });
            }
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult updateSanPham([FromBody] SanPham sanPham)
        {
            try
            {
                SanPham sp = db.SanPhams.FirstOrDefault(x => x.id_san_pham == sanPham.id_san_pham);
                
                if (sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                if (sanPham.id_danh_muc != 0)
                {
                    sp.id_danh_muc = sanPham.id_danh_muc;
                }
                if (sanPham.ten_sp != null)
                {
                    sp.ten_sp = sanPham.ten_sp;
                }
                if (sanPham.so_luong != 0)
                {
                    sp.so_luong = sanPham.so_luong;
                }
                if (sanPham.gia_sp != 0)
                {
                    sp.gia_sp = sanPham.gia_sp;
                }
                if(sanPham.phan_tram_km != 0)
                {
                    sp.phan_tram_km = sanPham.phan_tram_km;
                }
                if(sanPham.gia_km != 0)
                {
                    sp.gia_km = sanPham.gia_km;
                }
                if (sanPham.mo_ta != null)
                {
                    sp.mo_ta = sanPham.mo_ta;
                }
                if (sanPham.url_hinh_chinh != null)
                {
                    sp.url_hinh_chinh = sanPham.url_hinh_chinh;
                }
                
                db.SubmitChanges();
                var tam = new
                {
                    id_san_pham = sp.id_san_pham,
                    id_danh_muc = sp.id_danh_muc,
                    ten_sp = sp.ten_sp,
                    so_luong = sp.so_luong,
                    url_hinh_chinh = "http://www.3anhem.somee.com" + sp.url_hinh_chinh,
                    mo_ta = sp.mo_ta,
                    phan_tram_km = sp.phan_tram_km,
                    gia_sp = sp.gia_sp,
                    gia_km = sp.gia_km,
                };
                return Ok(tam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult deleteSanPham(int id)
        {
            try
            {
                SanPham sp = db.SanPhams.FirstOrDefault(x => x.id_san_pham == id);

                if(sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.SanPhams.DeleteOnSubmit(sp);
                db.SubmitChanges();
                return Ok(true);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("test")]
        public IHttpActionResult test()
        {
            try
            {
                List<SanPham> ls = db.SanPhams.ToList();
                string m = ls[0].mo_ta;
                for(int i = 0; i<ls.Count; i++)
                {
                    SanPham a = db.SanPhams.FirstOrDefault(e => e.id_san_pham == ls[i].id_san_pham);
                    a.mo_ta = m;
                    db.SubmitChanges();
                }
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
