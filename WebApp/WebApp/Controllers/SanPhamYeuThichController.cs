using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class SanPhamYeuThichController : ApiController
    {
        [HttpGet]
        public List<SanPhamYeuThich> getSanPhamYeuThichList()
        {
            QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();          
            return db.SanPhamYeuThiches.ToList();
        }
        [HttpPost]
        public IHttpActionResult insertNewSanPhamYeuThich([FromBody] SanPhamYeuThich sanPhamYeuThich)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_san_pham == sanPhamYeuThich.id_san_pham && x.id_khach_hang == sanPhamYeuThich.id_khach_hang);
                if(sp!= null)
                {
                    return Ok(false);
                }
                db.SanPhamYeuThiches.InsertOnSubmit(sanPhamYeuThich);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IHttpActionResult deleteSanPhamYeuThich([FromBody]SanPhamYeuThich sanPhamYeuThich)
        {
            try
            {
                QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
                SanPhamYeuThich sp = db.SanPhamYeuThiches.FirstOrDefault(x => x.id_yeu_thich == sanPhamYeuThich.id_yeu_thich);
                db.SanPhamYeuThiches.DeleteOnSubmit(sp);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
