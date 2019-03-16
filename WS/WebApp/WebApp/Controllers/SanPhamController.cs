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
                SanPham tam = new SanPham();
                if (sp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                tam.id_san_pham = sp.id_san_pham;
                tam.id_danh_muc = sp.id_danh_muc;
                tam.ten_sp = sp.ten_sp;
                tam.so_luong = sp.so_luong;
                tam.url_hinh_chinh = sp.url_hinh_chinh;
                tam.mo_ta = sp.mo_ta;
                tam.phan_tram_km = sp.phan_tram_km;
                tam.gia_sp = sp.gia_sp;
                tam.gia_km = sp.gia_km;
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
                tam.url_hinh_chinh = sp.url_hinh_chinh;
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
                List<SanPham> lsp = db.SanPhams.Where(x => x.id_danh_muc == id).ToList().Select(e => {
                    e.DanhMucSanPham = null;
                    e.SanPhamYeuThiches = null;
                    e.ChiTietDonHangs = null;
                    return e;
                }).ToList();
                if (lsp.Count==0) 
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                
                return Ok(lsp);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //lay danh sach san pham co chua thuoc tinh yeu thich hay ko
        //[HttpGet]
        //[ActionName("TheoDanhMuc")]
        //public IHttpActionResult ListSanPham(int idDanhMuc, int idKhachHang)
        //{
        //    try
        //    {
        //        List<SanPham> lsp = db.SanPhams.Where(x => x.id_danh_muc == idDanhMuc).ToList().Select(e => {
        //            e.DanhMucSanPham = null;
        //            e.SanPhamYeuThiches = null;
        //            e.ChiTietDonHangs = null;
        //            return e;
        //        }).ToList();
        //        if (lsp.Count == 0)
        //        {
        //            return StatusCode(HttpStatusCode.NoContent);
        //        }
        //        for(int i= 0; i<lsp.Count; i++)
        //        {
        //            SanPhamYeuThich spyt = db.SanPhamYeuThiches.FirstOrDefault(e => e.id_san_pham == lsp[i].id_san_pham && e.id_khach_hang == idKhachHang);
        //            int idlove = 0;
        //            var sanPhamCoYT = new
        //            {
        //                sanPham = lsp[i],
        //                idYeuThich = idlove
        //            };
                    
        //        }
                
        //        return Ok(lsp);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost]
        [ActionName("getPhanTrang")]
        public IHttpActionResult getSanPham10([FromBody] PhanTrang phamTrang)// phanTrang{id, Trang, Size}
        {
            try
            {
                List<SanPham> lsp = db.SanPhams.Where(x => x.id_danh_muc == phamTrang.id).ToPagedList(phamTrang.trang, phamTrang.size).ToList().Select(e =>
                {
                    e.DanhMucSanPham = null;
                    e.SanPhamYeuThiches = null;
                    e.ChiTietDonHangs = null;
                    return e;
                }).ToList();               
                if (lsp.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(lsp);
            }catch(Exception ex)
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
                List<SanPham> list = db.SanPhams.ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                int a = list.Count;

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
                        url_hinh_chinh = list[i].url_hinh_chinh,
                        mo_ta = list[i].mo_ta,
                        phan_tram_km = list[i].phan_tram_km,
                        gia_sp = list[i].gia_sp,
                        gia_km = list[i].gia_km
                    };
                    lds.Add(sp);
                }
                var tam = new
                {
                    cout = a,
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
                List<SanPham> list = db.SanPhams.ToList().ToPagedList(page,size).ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                List<dynamic> lds = new List<dynamic>();
                for (int i = 0; i < list.Count; i++)
                {
                    var sp = new
                    {
                        id_san_pham = list[i].id_san_pham,
                        id_danh_muc = list[i].id_danh_muc,
                        ten_sp = list[i].ten_sp,
                        so_luong = list[i].so_luong,
                        url_hinh_chinh = list[i].url_hinh_chinh,
                        mo_ta = list[i].mo_ta,
                        phan_tram_km = list[i].phan_tram_km,
                        gia_sp = list[i].gia_sp,
                        gia_km = list[i].gia_km
                    };
                    lds.Add(sp);
                }
                return Ok(lds);
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
                string hinh = sanPham.url_hinh_chinh;
                sanPham.url_hinh_chinh = "";

                db.SanPhams.InsertOnSubmit(sanPham);
                db.SubmitChanges();


                byte[] imageBytes = Convert.FromBase64String(hinh);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string fileName = "sanpham_" + sanPham.id_san_pham + ".png";
                image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/SanPham"), fileName));
                SanPham tam = db.SanPhams.FirstOrDefault(e => e.id_san_pham == sanPham.id_san_pham);

                tam.url_hinh_chinh = "~/hinh/SanPham" + fileName;

                db.SubmitChanges();

                tam.SanPhamYeuThiches = null;
                tam.DanhMucSanPham = null;
                tam.ChiTietDonHangs = null;
                return Ok(tam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return Ok(sp);
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
    }
}
