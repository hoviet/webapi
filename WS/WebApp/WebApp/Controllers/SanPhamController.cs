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
                    return NotFound();
                }
                sp.SanPhamYeuThiches = null;
                sp.DanhMucSanPham = null;
                sp.ChiTietDonHangs = null;
                return Ok(sp);
            } catch (Exception ex)
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
                if (lsp == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < lsp.Count; i++)
                {
                    lsp[i].DanhMucSanPham = null;
                    lsp[i].SanPhamYeuThiches = null;
                    lsp[i].ChiTietDonHangs = null;
                }
                return Ok(lsp);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getPhanTrang")]
        public IHttpActionResult getSanPham10([FromBody] PhanTrang phamTrang)// phanTrang{idDanhMuc, Trang, Size}
        {
            try
            {
                List<SanPham> lsp = db.SanPhams.Where(x => x.id_danh_muc == phamTrang.id).ToPagedList(phamTrang.trang, phamTrang.size).ToList();               
                if (lsp == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < lsp.Count; i++)
                {
                    lsp[i].DanhMucSanPham = null;
                    lsp[i].SanPhamYeuThiches = null;
                    lsp[i].ChiTietDonHangs = null;
                }
                return Ok(lsp);
            }catch(Exception ex)
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
                db.SanPhams.InsertOnSubmit(sanPham);
                db.SubmitChanges();
                //sanPham.url_hinh_chinh=
                return Ok(sanPham);
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
                    return NotFound();
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
                    return NotFound();
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
