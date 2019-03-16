using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class TinhTrangDonHangController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("danhsach")] 
        public IHttpActionResult getTinhTrangDonHangList()
        {
            try
            {
                List<TinhTrangDonHang> list = db.TinhTrangDonHangs.ToList();
                if(list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                List<dynamic> lds = new List<dynamic>();
                for(int i = 0; i <list.Count; i++)
                {
                    var tam = new
                    {
                        id_tinh_trang = list[i].id_tinh_trang,
                        tinh_trang_don_hang = list[i].tinh_trang_don_hang,
                        ghi_chu = list[i].ghi_chu
                    };
                    lds.Add(tam);
                }

                return Ok(lds);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("LayMot")] //truyen vao id_tinh_trang
        public IHttpActionResult getTinhTrangDonHang(int id)
        {
            try
            {
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == id);
                if(tr == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                var tam = new
                {
                    id_tinh_trang = tr.id_tinh_trang,
                    tinh_trang_don_hang = tr.tinh_trang_don_hang,
                    ghi_chu = tr.ghi_chu
                };
                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("them")] //truyen vao cac thuoc tinh
        public IHttpActionResult insertNewTinhTrangDonHang([FromBody] TinhTrangDonHang tinhTrangDonHang)
        {
            try
            {
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(e => e.tinh_trang_don_hang.Equals(tinhTrangDonHang.tinh_trang_don_hang));
                if(tr == null)
                {
                    var tam = new
                    {
                        id_tinh_trang = tr.id_tinh_trang,
                        tinh_trang_don_hang = tr.tinh_trang_don_hang,
                        ghi_chu = tr.ghi_chu
                    };
                    return Ok(tam);
                }

                db.TinhTrangDonHangs.InsertOnSubmit(tinhTrangDonHang);
                db.SubmitChanges();
                var tinhTrang = new
                {
                    id_tinh_trang = tinhTrangDonHang.id_tinh_trang,
                    tinh_trang_don_hang = tinhTrangDonHang.tinh_trang_don_hang,
                    ghi_chu = tinhTrangDonHang.ghi_chu
                };
                return Ok(tinhTrang);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ActionName("update")] //truyen vao id_tinh_trang vaf cac thuoc tinh muon sua
        public IHttpActionResult updateTinhTrangDonHang([FromBody] TinhTrangDonHang tinhTrangDonHang)
        {
            try
            {
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == tinhTrangDonHang.id_tinh_trang);

                if(tr == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                if(tinhTrangDonHang.tinh_trang_don_hang != null)
                {
                    tr.tinh_trang_don_hang = tinhTrangDonHang.tinh_trang_don_hang;
                }
                if(tinhTrangDonHang.ghi_chu != null)
                {
                    tr.ghi_chu = tinhTrangDonHang.ghi_chu;
                }

                db.SubmitChanges();
                var tam = new
                {
                    id_tinh_trang = tr.id_tinh_trang,
                    tinh_trang_don_hang = tr.tinh_trang_don_hang,
                    ghi_chu = tr.ghi_chu
                };
                return Ok(tam);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("delete")] //truyen vao id_tinh_trang
        public IHttpActionResult deleteTinhTrangDonHang(int id)
        {
            try
            {
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == id);

                if(tr == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }

                db.TinhTrangDonHangs.DeleteOnSubmit(tr);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
