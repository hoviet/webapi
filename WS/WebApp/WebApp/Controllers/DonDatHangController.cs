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
    public class DonDatHangController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("getList")]
       public IHttpActionResult getListHoaDon([FromBody] PhanTrang phanTrang)
        {
            try
            {
                List<DonDatHang> list = db.DonDatHangs.Where(x => x.id_khach_hang == phanTrang.id).ToPagedList(phanTrang.trang, phanTrang.size).ToList();
                if(list == null)
                {
                    return NotFound();
                }
                for(int i = 0; i <list.Count; i++)
                {
                    list[i].KhachHang.SanPhamYeuThiches = null;
                    list[i].ChiTietDonHangs = null;
                }
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("getMotHoaDon")]
        public IHttpActionResult getMotHoaDon(int id)
        {
            try
            {
                DonDatHang hd = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
                HoaDon hoaDonTam = new HoaDon();
                if(hd == null)
                {
                    return NotFound();
                }
                hoaDonTam.id = hd.id_don_hang;
                hoaDonTam.idKhachHang = hd.id_khach_hang;
                hoaDonTam.idTinhTrang = hd.id_tinh_trang;
                hoaDonTam.ngayLap = hd.ngay_lap;
                hoaDonTam.noiNhan = hd.noi_nhan;
                hoaDonTam.soDT = hd.so_dt_nguoi_nhan;
                hoaDonTam.tongGia =(float) hd.tong_tien;
                hoaDonTam.ghiChu = hd.ghi_chu;
                return Ok(hoaDonTam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         //hien thi mot hoa don dat hang chi tiet
        [HttpGet]
        [ActionName("hoaDon")]
        public IHttpActionResult getHoaDon(int idDonHang, int idKhachHang) //id
        {
            try
            {
                HienThiDonHang ctDonHang = new HienThiDonHang();
                DonDatHang donHang = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == idDonHang && x.id_khach_hang == idKhachHang);
                if(donHang == null)
                {
                    return NotFound();
                }
                ctDonHang.idDonDatHang = donHang.id_don_hang;
                ctDonHang.trangThai = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == donHang.id_tinh_trang).tinh_trang_don_hang;
                ctDonHang.ngayLap = donHang.ngay_lap;
                ctDonHang.tenNguoiNhan = db.KhachHangs.FirstOrDefault(x => x.id_khach_hang == donHang.id_khach_hang).ten_nguoi_dung;
                ctDonHang.soDT ="0"+ donHang.so_dt_nguoi_nhan;
                ctDonHang.diaChi = donHang.noi_nhan;
                // add danh sach san pham
                List<ChiTietDonHang> list = db.ChiTietDonHangs.Where(x => x.id_don_hang == idDonHang).ToList();
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
        //tao don dat hang moi va luu cao xsdl
        [HttpPost]
        [ActionName("themHoaDon")]
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
                ddh.noi_nhan = donHang.noiNhan;
                ddh.ghi_chu = donHang.ghiChu;
                db.DonDatHangs.InsertOnSubmit(ddh);
                db.SubmitChanges();
                //tao chi tiet don dat hang tuong ung
                DonDatHang tam = db.DonDatHangs.Where(x => x.id_khach_hang == donHang.idKhachHang).ToList().Last();

                for(int i = 0; i <donHang.danhSachSanPham.Count; i++)
                {
                    ChiTietDonHang ctDonHang = new ChiTietDonHang();
                    int id = donHang.danhSachSanPham[i].idSanPhan;
                    ctDonHang.id_don_hang = tam.id_don_hang;
                    ctDonHang.id_san_pham = 1;
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
        [HttpPost]
        [ActionName("insert")]
        public IHttpActionResult insertDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
                db.DonDatHangs.InsertOnSubmit(donDatHang);
                db.SubmitChanges();
                return Ok(true);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult updateDonDatHang([FromBody] DonDatHang donDatHang)
        {
            try
            {
                DonDatHang ddh = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == donDatHang.id_don_hang);

                if(ddh == null)
                {
                    return NotFound();
                }
                if(donDatHang.id_tinh_trang != 0)
                {
                    ddh.id_tinh_trang = donDatHang.id_tinh_trang;
                }
                if(donDatHang.ngay_lap.Equals("0001 - 01 - 01T00: 00:00"))
                {
                    ddh.ngay_lap = donDatHang.ngay_lap;
                }
                if(donDatHang.tong_tien != 0)
                {
                    ddh.tong_tien = donDatHang.tong_tien;
                }
                if(donDatHang.so_dt_nguoi_nhan != 0)
                {
                    ddh.so_dt_nguoi_nhan = donDatHang.so_dt_nguoi_nhan;
                }
                if(donDatHang.noi_nhan != null)
                {
                    ddh.noi_nhan = donDatHang.noi_nhan;
                }
                if(donDatHang.ghi_chu != null)
                {
                    ddh.ghi_chu = donDatHang.ghi_chu;
                }

                db.SubmitChanges();
                return Ok(ddh);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult deleteDonDatHang(int id)
        {
            try
            {
                DonDatHang ddh = db.DonDatHangs.FirstOrDefault(x => x.id_don_hang == id);
                if(ddh == null)
                {
                    return NotFound();
                }
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
