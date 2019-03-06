﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PagedList;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DonDatHangController : ApiController
    {
       private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        //lay danh sach don dat hang doi voi 1 khach hang
        [HttpPost]
        [ActionName("getList")]
       public IHttpActionResult getListHoaDon([FromBody] PhanTrang phanTrang)
        {
            try
            {
                //
                List<DonDatHang> list = db.DonDatHangs.Where(x => x.id_khach_hang == phanTrang.id).ToPagedList(phanTrang.trang, phanTrang.size).ToList();
                List<HTListHoaDon> lhd = new List<HTListHoaDon>();
                if(list == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for(int i = 0; i <list.Count; i++)
                {
                    HTListHoaDon hoaDonTam = new HTListHoaDon();
                    // tao dia chi noi nhan hang
                    DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == list[i].id_dia_chi);
                    TinhThanh tinh = db.TinhThanhs.FirstOrDefault(x => x.ma_tinh == dc.id_tinh);
                    QuanHuyen quan = db.QuanHuyens.FirstOrDefault(x => x.ma_quan_huyen == dc.id_quan);
                    XaPhuong xa = db.XaPhuongs.FirstOrDefault(x => x.ma_xa_phuong == dc.id_xa_phuong);
                    string diaChi = "" + dc.dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten;

                    hoaDonTam.id = list[i].id_don_hang;
                    hoaDonTam.KhachHang = db.KhachHangs.FirstOrDefault(e => e.id_khach_hang == list[i].id_khach_hang).ten_nguoi_dung;
                    hoaDonTam.TinhTrang = db.TinhTrangDonHangs.FirstOrDefault(e => e.id_tinh_trang == list[i].id_tinh_trang).tinh_trang_don_hang;
                    hoaDonTam.ngayLap = list[i].ngay_lap;
                    hoaDonTam.DiaChi = diaChi;
                    hoaDonTam.soDT = list[i].so_dt_nguoi_nhan;
                    hoaDonTam.tongGia = (float)list[i].tong_tien;
                    hoaDonTam.ghiChu = list[i].ghi_chu;
                    lhd.Add(hoaDonTam);
                }
                return Ok(lhd);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        //hien thi mot hoa don dat hang chi tiet
        [HttpGet]
        [ActionName("hoaDon")]
        public IHttpActionResult getHoaDon(int id) //id
        {
            try
            {
                HienThiDonHang ctDonHang = new HienThiDonHang();              
                string diaChi = "";
                DonDatHang donHang = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(x => x.id == donHang.id_dia_chi);
                TinhThanh tinh = db.TinhThanhs.FirstOrDefault(x => x.ma_tinh == dc.id_tinh);
                QuanHuyen quan = db.QuanHuyens.FirstOrDefault(x => x.ma_quan_huyen == dc.id_quan);
                XaPhuong xa = db.XaPhuongs.FirstOrDefault(x => x.ma_xa_phuong == dc.id_xa_phuong);
                if (donHang == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                ctDonHang.idDonDatHang = donHang.id_don_hang;
                ctDonHang.trangThai = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == donHang.id_tinh_trang).tinh_trang_don_hang;
                ctDonHang.ngayLap = donHang.ngay_lap;
                ctDonHang.tenNguoiNhan = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == donHang.id_khach_hang).ten_nguoi_dung;
                ctDonHang.soDT ="0"+ donHang.so_dt_nguoi_nhan;
                diaChi = "" + dc.dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten;
                ctDonHang.diaChi = diaChi;
                // add danh sach san pham
                List<ChiTietDonHang> list = db.ChiTietDonHangs.Where(x => x.id_don_hang == id).ToList();
                List<DSSanPham> lDanhSanPham = new List<DSSanPham>();
                for(int i = 0; i< list.Count; i++)
                {
                    DSSanPham dsp = new DSSanPham();                   
                    dsp.soLuong = list[i].so_luong;
                    dsp.tongGia =(float) list[i].tong_tien;
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
                ctDonHang.danhSachHang = lDanhSanPham;
                return Ok(ctDonHang);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("TaoHoaDon")]
        public IHttpActionResult themHoaDon([FromBody] TaoDonHang donHang)
        {
            try
            {
                //tao 1 don dat hang
                DonDatHang ddh = new DonDatHang();
                ddh.id_khach_hang = donHang.idKhachHang;
                ddh.id_tinh_trang = donHang.idTinhTrang;
                ddh.ngay_lap = donHang.ngayLap;
                ddh.tong_tien = donHang.tongTien;
                ddh.so_dt_nguoi_nhan = donHang.soDT;
                ddh.id_dia_chi = donHang.idNoiNhan;
                ddh.ghi_chu = donHang.ghiChu;
                db.DonDatHangs.InsertOnSubmit(ddh);
                db.SubmitChanges();
                //tao chi tiet don dat hang tuong ung
                DonDatHang tam = db.DonDatHangs.Where(x => x.id_khach_hang == donHang.idKhachHang).ToList().Last();

                for(int i = 0; i <donHang.danhSachSanPham.Count; i++)
                {
                    ChiTietDonHang ctDonHang = new ChiTietDonHang();
                    int id = donHang.danhSachSanPham[i].idSanPham;
                    ctDonHang.id_don_hang = tam.id_don_hang;
                    ctDonHang.id_san_pham = donHang.danhSachSanPham[i].idSanPham;
                    ctDonHang.gia_km = donHang.danhSachSanPham[i].giaKM;
                    ctDonHang.so_luong = donHang.danhSachSanPham[i].soLuong;
                    ctDonHang.tong_tien = donHang.danhSachSanPham[i].tongGia;
                    ctDonHang.thoi_gian_lap = donHang.ngayLap;

                    db.ChiTietDonHangs.InsertOnSubmit(ctDonHang);
                    db.SubmitChanges();
                }
                
                return Ok(donHang);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Xoa don dat hang xoa chi tiet don dat hang
        [HttpGet]
        [ActionName("huyHoaDon")]
        public IHttpActionResult deleteDonDatHang(int id)
        {
            try
            {
                DonDatHang ddh = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
                if (ddh == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                //xoa chi tiet don dat hang
                List<ChiTietDonHang> ct = db.ChiTietDonHangs.Where(x => x.id_don_hang == id).ToList();
                if (ct.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for (int i = 0; i < ct.Count; i++)
                {
                    db.ChiTietDonHangs.DeleteOnSubmit(ct[i]);
                    db.SubmitChanges();
                }  
                //xoa don dat hang
                db.DonDatHangs.DeleteOnSubmit(ddh);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
