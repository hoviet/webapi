﻿using System;
using System.Collections.Generic;
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
    public class DiaChiKhachHangController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("DanhSach")]
        public IHttpActionResult danhSach(int id)
        {
            try
            {
                List<DiaChiKhachHang> list = db.DiaChiKhachHangs.Where(e => e.id_khach_hang == id).ToList().Select(e => { e.DonDatHangs = null; return e; }).ToList();
                List<HTDiaChi> ldt = new List<HTDiaChi>();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                } 
                for(int i = 0; i < list.Count; i++)
                {
                    HTDiaChi tam = new HTDiaChi();
                   
                    TinhThanh tinh = db.TinhThanhs.FirstOrDefault(x => x.ma_tinh == list[i].id_tinh);
                    QuanHuyen quan = db.QuanHuyens.FirstOrDefault(x => x.ma_quan_huyen == list[i].id_quan);
                    XaPhuong xa = db.XaPhuongs.FirstOrDefault(x => x.ma_xa_phuong == list[i].id_xa_phuong);
                    string diaChi = "" + list[i].dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten;

                    tam.loai = list[i].loai;
                    tam.soDt = list[i].so_dt;
                    tam.idKhachHang = list[i].id_khach_hang;
                    tam.tenKhachHang = list[i].ten_khach_hang;
                    tam.idDiaChiKhachHang = list[i].id;
                    tam.tenDiaChi = diaChi;
                    tam.tinh = tinh.ten;
                    tam.quanHuyen = quan.ten_quan_huyen;
                    tam.xaPhuong = xa.ten;
                    tam.diaChi = list[i].dia_chi;
                    ldt.Add(tam);
                }
                return Ok(ldt);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("layMotDiaChi")]
        public IHttpActionResult layMotDiaChi(int id)
        {
            try
            {
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == id);
                CTDiaChi tam = new CTDiaChi();
                if(dc == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                tam.idXaPhuong = dc.id_xa_phuong;
                tam.tenXaPhuong = db.XaPhuongs.FirstOrDefault(e => e.ma_xa_phuong == dc.id_xa_phuong).ten;

                List<QuanHuyen> listQuanHuyen = db.QuanHuyens.Where(e => e.ma_tinh == dc.id_tinh).ToList().Select(e => { e.TinhThanh = null; e.XaPhuongs = null; e.DiaChiKhachHangs = null; return e; }).ToList();
                List<XaPhuong> listXaPhuong = db.XaPhuongs.Where(e => e.ma_quan_huyen == dc.id_quan).ToList().Select(e => { e.QuanHuyen = null; e.DiaChiKhachHangs = null; return e; }).ToList();
                
                tam.diaChi = dc.dia_chi;
                tam.id = dc.id;
                tam.idKhachHang = dc.id_khach_hang;
                tam.tenKhachHang = dc.ten_khach_hang;
                tam.soDT = dc.so_dt;
                tam.idQuanHuyen = dc.id_quan;
                tam.tenQuanHuyen = db.QuanHuyens.FirstOrDefault(e => e.ma_quan_huyen == dc.id_quan).ten_quan_huyen;
                tam.idTinh = dc.id_tinh;
                tam.tenTinh = db.TinhThanhs.FirstOrDefault(e => e.ma_tinh == dc.id_tinh).ten;
                tam.dsQuanHuyen = listQuanHuyen;
                tam.dsXaPhuong = listXaPhuong;

                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("taoDiaChiMoi")]
        public IHttpActionResult insert([FromBody] DiaChiKhachHang diaChi)
        {
            try
            {
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e =>e.so_dt==diaChi.so_dt && e.id_khach_hang == diaChi.id_khach_hang && e.id_xa_phuong == diaChi.id_xa_phuong && e.dia_chi.Equals(diaChi.dia_chi));
                if(dc != null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.DiaChiKhachHangs.InsertOnSubmit(diaChi);
                db.SubmitChanges();

                DiaChiKhachHang tam = new DiaChiKhachHang();
                tam.id = diaChi.id;
                tam.id_khach_hang = diaChi.id_khach_hang;
                tam.id_quan = diaChi.id_quan;
                tam.id_tinh = diaChi.id_tinh;
                tam.id_xa_phuong = diaChi.id_xa_phuong;
                tam.loai = diaChi.loai;
                tam.dia_chi = diaChi.dia_chi;
                tam.so_dt = diaChi.so_dt;
                tam.ten_khach_hang = diaChi.ten_khach_hang;
                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [ActionName("update")]
        public IHttpActionResult update([FromBody] DiaChiKhachHang diaChi)
        {
            try
            {
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == diaChi.id);
                if(dc == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                dc.id_quan = diaChi.id_quan;
                dc.id_tinh = diaChi.id_tinh;
                dc.id_xa_phuong = diaChi.id_xa_phuong;
                dc.dia_chi = diaChi.dia_chi;
                dc.loai = diaChi.loai;
                db.SubmitChanges();
                return Ok(dc);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("XoaDiaChi")]
        public IHttpActionResult delete(int id)
        {
            try
            {
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == id);
                if(dc == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.DiaChiKhachHangs.DeleteOnSubmit(dc);
                db.SubmitChanges();
                return Ok(dc);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
