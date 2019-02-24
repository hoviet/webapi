using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class DanhMucSanPhamController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("getTenLoaiSanPham")]
        public IHttpActionResult getSanPhamList([FromBody] DanhMucSanPham loaiSanPham)
        {
            try
            {
                List<DanhMucSanPham> listTam = db.DanhMucSanPhams.ToList();
                if(listTam == null)
                {
                    return NotFound();
                }
                for(int i = 0; i < listTam.Count; i++)
                {
                    listTam[i].SanPhams = null;
                }
                return Ok(listTam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getMotLoaiSanPham")]
        public IHttpActionResult getMotLoaiSanPham([FromBody] DanhMucSanPham loaisp)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == loaisp.id_danh_muc);
                if(dmsp == null)
                {
                    return NotFound();
                }
                dmsp.SanPhams = null;
                return Ok(dmsp);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult inserNewDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPham)
        {
            try
            {
                db.DanhMucSanPhams.InsertOnSubmit(danhMucSanPham);
                db.SubmitChanges();
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult updaeDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPham)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == danhMucSanPham.id_danh_muc);

                if(dmsp == null)
                {
                    return NotFound();
                }
                if(danhMucSanPham.ten_danh_muc != null)
                {
                    dmsp.ten_danh_muc = danhMucSanPham.ten_danh_muc;
                }
                if(danhMucSanPham.url_hinh != null)
                {
                    dmsp.url_hinh = danhMucSanPham.url_hinh;
                }

                db.SubmitChanges();
                return Ok(danhMucSanPham);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult deleteDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPham)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == danhMucSanPham.id_danh_muc);

                if(dmsp == null)
                {
                    return NotFound();
                }

                db.DanhMucSanPhams.DeleteOnSubmit(danhMucSanPham);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
