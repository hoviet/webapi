using PagedList;
using System;
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
                List<DiaChiKhachHang> list = db.DiaChiKhachHangs.Where(e => e.id_khach_hang == id && e.trang_thai == true).ToList().Select(e => { e.DonDatHangs = null; return e; }).ToList();
                List<dynamic> ldt = new List<dynamic>();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                } 
                for(int i = 0; i < list.Count; i++)
                {
                    List<QuanHuyen> listQuanHuyen = db.QuanHuyens.Where(e => e.ma_tinh == list[i].id_tinh).ToList();
                    List<dynamic> listQuan = new List<dynamic>();
                    for(int j = 0; j<listQuanHuyen.Count; j++)
                    {
                        var a = new
                        {
                            ma_quan_huyen = listQuanHuyen[i].ma_quan_huyen,
                            ten_quan_huyen = listQuanHuyen[i].ten_quan_huyen,
                            loai = listQuanHuyen[i].loai,
                            ma_tinh = listQuanHuyen[i].ma_tinh,
                        };
                        listQuan.Add(a);
                    }
                    List<XaPhuong> listXaPhuong = db.XaPhuongs.Where(e => e.ma_quan_huyen == list[i].id_quan).ToList();
                    List<dynamic> lx = new List<dynamic>();
                    for (int k = 0; k < listXaPhuong.Count; k++)
                    {
                        var b = new
                        {
                            ma_quan_huyen = listXaPhuong[k].ma_quan_huyen,
                            ten = listXaPhuong[i].ten,
                            loai = listXaPhuong[i].loai,
                            ma_xa_phuong = listXaPhuong[i].ma_xa_phuong,
                        };
                        lx.Add(b);
                    }
                    TinhThanh tinh = db.TinhThanhs.FirstOrDefault(x => x.ma_tinh == list[i].id_tinh);
                    QuanHuyen quan = db.QuanHuyens.FirstOrDefault(x => x.ma_quan_huyen == list[i].id_quan);
                    XaPhuong xa = db.XaPhuongs.FirstOrDefault(x => x.ma_xa_phuong == list[i].id_xa_phuong);
                    string diaChi = list[i].dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten;
                    var tam = new
                    {
                        idXaPhuong = list[i].id_xa_phuong,
                        tenXaPhuong = xa.ten,
                        diaChi = list[i].dia_chi,
                        id = list[i].id,
                        idKhachHang = list[i].id_khach_hang,
                        tenKhachHang = list[i].ten_khach_hang,
                        soDT = list[i].so_dt,
                        idQuanHuyen = list[i].id_quan,
                        tenQuanHuyen = quan.ten_quan_huyen,
                        idTinh = list[i].id_tinh,
                        tenTinh = tinh.ten,
                        diaChiCuThe = list[i].dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten,
                        dsQuanHuyen = listQuan,
                        dsXaPhuong = lx
                       
                    };
                    ldt.Add(tam);
                }
                return Ok(ldt);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult alldanhSach()
        {
            try
            {
                List <DiaChiKhachHang> ldc = db.DiaChiKhachHangs.Where(e=>e.trang_thai == true).ToList();
                if(ldc.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                int a = ldc.Count; // chieu dai cua danh sach
                ldc.ToPagedList(1, 10).ToList();
                List<dynamic> list = new List<dynamic>();
                for(int i = 0; i <ldc.Count; i++)
                {
                    TinhThanh tinh = db.TinhThanhs.FirstOrDefault(x => x.ma_tinh == ldc[i].id_tinh);
                    QuanHuyen quan = db.QuanHuyens.FirstOrDefault(x => x.ma_quan_huyen == ldc[i].id_quan);
                    XaPhuong xa = db.XaPhuongs.FirstOrDefault(x => x.ma_xa_phuong == ldc[i].id_xa_phuong);
                    string diaChi = "" + ldc[i].dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten;
                    var tam = new
                    {
                        idDiaChi = ldc[i].id,
                        tenKhachHang = ldc[i].ten_khach_hang,
                        idKhachHang = ldc[i].id_khach_hang,
                        soDT = ldc[i].so_dt,
                        diaChi = diaChi,

                    };
                    list.Add(tam);
                }
                var obj = new
                {
                    danhSach = list,
                    count = a
                };
                return Ok(obj);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("danhsach")]
        public IHttpActionResult allDS(int page, int size)
        {
            try
            {
                List<DiaChiKhachHang> ldc = db.DiaChiKhachHangs.Where(e => e.trang_thai == true).ToList().ToPagedList(page, size).ToList();
                if (ldc.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                List<dynamic> list = new List<dynamic>();
                for (int i = 0; i < ldc.Count; i++)
                {
                    TinhThanh tinh = db.TinhThanhs.FirstOrDefault(x => x.ma_tinh == ldc[i].id_tinh);
                    QuanHuyen quan = db.QuanHuyens.FirstOrDefault(x => x.ma_quan_huyen == ldc[i].id_quan);
                    XaPhuong xa = db.XaPhuongs.FirstOrDefault(x => x.ma_xa_phuong == ldc[i].id_xa_phuong);
                    string diaChi = "" + ldc[i].dia_chi + ", " + xa.ten + ", " + quan.ten_quan_huyen + ", " + tinh.ten;
                    var tam = new
                    {
                        idDiaChi = ldc[i].id,
                        tenKhachHang = ldc[i].ten_khach_hang,
                        idKhachHang = ldc[i].id_khach_hang,
                        soDT = ldc[i].so_dt,
                        diaChi = diaChi,

                    };
                    list.Add(tam);
                }
                return Ok(list);
            }
            catch (Exception ex)
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
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == id && e.trang_thai == true);
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
                diaChi.trang_thai = true;
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
                DiaChiKhachHang dc = db.DiaChiKhachHangs.FirstOrDefault(e => e.id == diaChi.id );
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
                var tam = new
                {
                    id = dc.id,
                    id_khach_hang = dc.id_khach_hang,
                    ten_khach_hang = dc.ten_khach_hang,
                    so_dt = dc.so_dt,
                    id_quan = dc.id_quan,
                    id_tinh = dc.id_tinh,
                    id_xa_phuong = dc.id_xa_phuong,
                    dia_chi = dc.dia_chi,
                    loai = dc.loai
                };
                return Ok(tam);
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
                dc.trang_thai = false;
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
