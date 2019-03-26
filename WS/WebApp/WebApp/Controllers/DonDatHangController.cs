using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using PagedList;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    
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
                    hoaDonTam.ngayLap = list[i].ngay_lap.ToShortDateString();
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
                ctDonHang.ngayLap = donHang.ngay_lap.ToShortDateString();
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
                    dsp.sanPham = sp;
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
        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult danhDachHoaDon()
        {
            try
            {
                List<DonDatHang> list = db.DonDatHangs.ToList();
                int a = list.Count;
                list.ToPagedList(1, 10).ToList();

                List<dynamic> lhd = new List<dynamic>();

                for(int i = 0; i<list.Count; i++)
                {
                    var tam = new
                    {
                        idDonHang = list[i].id_don_hang,
                        idKhachHang = list[i].id_khach_hang,
                        tenKhachHang = db.KhachHangs.FirstOrDefault(e => e.id_khach_hang == list[i].id_khach_hang).ten_nguoi_dung,
                        ngayLap = list[i].ngay_lap.ToShortDateString(),
                        idTinhTrang = list[i].id_tinh_trang,
                        soDT = list[i].so_dt_nguoi_nhan,
                        tinhTrang = db.TinhTrangDonHangs.FirstOrDefault(e => e.id_tinh_trang == list[i].id_tinh_trang).tinh_trang_don_hang,
                        tongTien = list[i].tong_tien
                    };
                    lhd.Add(tam);
                }
                var oj = new
                {
                    danhSach = lhd,
                    count = a
                };
                return Ok(oj);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult danhDachHoaDon(int page, int size)
        {
            try
            {
                List<DonDatHang> list = db.DonDatHangs.ToList().ToPagedList(page, size).ToList();
                List<dynamic> lhd = new List<dynamic>();

                for (int i = 0; i < list.Count; i++)
                {
                    var tam = new
                    {
                        idDonHang = list[i].id_don_hang,
                        idKhachHang = list[i].id_khach_hang,
                        tenKhachHang = db.KhachHangs.FirstOrDefault(e => e.id_khach_hang == list[i].id_khach_hang).ten_nguoi_dung,
                        ngayLap = list[i].ngay_lap.ToShortDateString(),
                        tinhTrang = db.TinhTrangDonHangs.FirstOrDefault(e => e.id_tinh_trang == list[i].id_tinh_trang).tinh_trang_don_hang,
                        idTinhTrang = list[i].id_tinh_trang,
                        soDT = list[i].so_dt_nguoi_nhan,
                        tongTien = list[i].tong_tien
                    };
                    lhd.Add(tam);
                }
                return Ok(lhd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //List<dynamic> b = new List<dynamic> { };
        [HttpPost]
        [ActionName("chuyentrangthai")]
        public IHttpActionResult chuyendoi(int idDonHang, int idTinhTrang)
        {
            try
            {
                DonDatHang donDH = db.DonDatHangs.FirstOrDefault(e => e.id_don_hang == idDonHang);
                if(donDH == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                donDH.id_tinh_trang = idTinhTrang;
                db.SubmitChanges();

                ThongBaoChuyenTT tb = new ThongBaoChuyenTT();
                tb.giuThongBao(donDH.id_khach_hang, donDH.id_tinh_trang);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
