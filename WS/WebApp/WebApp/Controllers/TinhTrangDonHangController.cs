using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class TinhTrangDonHangController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("getListTinhTrang")] 
        public IHttpActionResult getTinhTrangDonHangList()
        {
            try
            {
                List<TinhTrangDonHang> list = db.TinhTrangDonHangs.ToList();
                if(list == null)
                {
                    return NotFound();
                }
                for(int i = 0;i <list.Count; i++)
                {
                    list[i].DonDatHangs = null;
                }
                return Ok(list);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("getTinhTrang")] //truyen vao id_tinh_trang
        public IHttpActionResult getTinhTrangDonHang(int id)
        {
            try
            {
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(x => x.id_tinh_trang == id);
                if(tr == null)
                {
                    return NotFound();
                }
                tr.DonDatHangs = null;
                return Ok(tr);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("insert")] //truyen vao cac thuoc tinh
        public IHttpActionResult insertNewTinhTrangDonHang([FromBody] TinhTrangDonHang tinhTrangDonHang)
        {
            try
            {
                db.TinhTrangDonHangs.InsertOnSubmit(tinhTrangDonHang);
                db.SubmitChanges();
                return Ok();
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
                    return NotFound();
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
                return Ok(tr);
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
                    return NotFound();
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
