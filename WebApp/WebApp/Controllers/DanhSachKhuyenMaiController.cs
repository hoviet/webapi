using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class DanhSachKhuyenMaiController : ApiController
    {
        [HttpGet]
        public List<DanhSachKhuyenMai> getKhuyenMaiList()
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
            return db.DanhSachKhuyenMais.ToList();
        }

        [HttpGet]
        public DanhSachKhuyenMai GetDanhSachKhuyenMai(int id)
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

            return db.DanhSachKhuyenMais.FirstOrDefault(x => x.id_khuyen_mai == id);
        }

        [HttpPost]
        public IHttpActionResult insertNewKhuyenMai([FromBody]DanhSachKhuyenMai khuyenMai)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

                db.DanhSachKhuyenMais.InsertOnSubmit(khuyenMai);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IHttpActionResult updateKhuyenMai([FromBody] DanhSachKhuyenMai khuyenMai)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                DanhSachKhuyenMai km = db.DanhSachKhuyenMais.FirstOrDefault(x => x.id_khuyen_mai == khuyenMai.id_khuyen_mai);
                if(km == null)
                {
                    return NotFound();
                }
                if(khuyenMai.ten_km != null)
                {
                    km.ten_km = khuyenMai.ten_km;
                }
                if(khuyenMai.phan_tram_km != 0)
                {
                    km.phan_tram_km = khuyenMai.phan_tram_km;
                }
                if(khuyenMai.t_bat_dau.Equals("0001 - 01 - 01T00: 00:00")){
                    km.t_bat_dau = khuyenMai.t_bat_dau;
                }
                if (khuyenMai.t_ket_thuc.Equals("0001 - 01 - 01T00: 00:00"))
                {
                    km.t_ket_thuc = khuyenMai.t_ket_thuc;
                }

                db.SubmitChanges();
                return Ok(km);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IHttpActionResult deleteKhuyenMai([FromBody] DanhSachKhuyenMai khuyenMai)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                DanhSachKhuyenMai km = db.DanhSachKhuyenMais.FirstOrDefault(x => x.id_khuyen_mai == khuyenMai.id_khuyen_mai);
                if(km == null)
                {
                    return NotFound();
                }
                db.DanhSachKhuyenMais.DeleteOnSubmit(khuyenMai);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
