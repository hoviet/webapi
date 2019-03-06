using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ChiTietDonHangController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("getListMotDon")]
        public IHttpActionResult getChiTietDonHang(int id)
        {
            try
            {
                List<ChiTietDonHang> list = db.ChiTietDonHangs.Where(x => x.id_don_hang == id).ToList();
                if(list == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<DSSanPham> lDanhSanPham = new List<DSSanPham>();
                for (int i = 0; i < list.Count; i++)
                {
                    DSSanPham dsp = new DSSanPham();
                    dsp.soLuong = list[i].so_luong;
                    dsp.tongGia = (float)list[i].tong_tien;
                    dsp.giaKM = (float)list[i].gia_km;
                    //gan san pham 
                    SanPham tam = db.SanPhams.FirstOrDefault(x => x.id_san_pham == list[i].id_san_pham);
                    SanPham sp = new SanPham();
                    sp.id_san_pham = tam.id_san_pham;
                    sp.id_danh_muc = tam.id_danh_muc;
                    sp.mo_ta = tam.mo_ta;
                    sp.phan_tram_km = tam.phan_tram_km;
                    sp.ten_sp = tam.ten_sp;
                    sp.url_hinh_chinh = tam.url_hinh_chinh;
                    sp.gia_sp = tam.gia_sp;
                    sp.gia_km = tam.gia_km;
                    dsp.sanPhan = sp;
                    lDanhSanPham.Add(dsp);
                }
                return Ok(lDanhSanPham);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult insertNewChiTietDonHang([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
                db.ChiTietDonHangs.InsertOnSubmit(chiTietDonHang);
                db.SubmitChanges();
                return Ok(chiTietDonHang);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult updateChiTietDonHang([FromBody] ChiTietDonHang chiTietDonHang)
        {
            try
            {
                ChiTietDonHang ct = db.ChiTietDonHangs.FirstOrDefault(x => x.id_don_hang == chiTietDonHang.id_don_hang && x.id_san_pham == chiTietDonHang.id_san_pham);
                if(ct == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                if(chiTietDonHang.so_luong != 0)
                {
                    ct.so_luong = chiTietDonHang.so_luong;
                }
                if(chiTietDonHang.gia_km != 0)
                {
                    ct.gia_km = chiTietDonHang.gia_km;
                }
                if(chiTietDonHang.tong_tien != 0)
                {
                    ct.tong_tien = chiTietDonHang.tong_tien;
                }
                if(chiTietDonHang.thoi_gian_lap.Equals("0001 - 01 - 01T00: 00:00"))
                {
                    ct.thoi_gian_lap = chiTietDonHang.thoi_gian_lap;
                }
                
                db.SubmitChanges();
                return Ok(ct);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("xoa")]
        public IHttpActionResult deleteChiTietSanPham(int idDonHang)
        {
            try
            {
                List< ChiTietDonHang> ct = db.ChiTietDonHangs.Where(x => x.id_don_hang == idDonHang).ToList();
                if(ct.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for(int i = 0; i< ct.Count; i++)
                {
                    db.ChiTietDonHangs.DeleteOnSubmit(ct[i]);
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
