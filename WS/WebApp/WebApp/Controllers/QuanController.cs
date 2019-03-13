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
    public class QuanController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("lay")]
        public IHttpActionResult layQuan(string idTinh)
        {
            try
            {
                List<QuanHuyen> list = db.QuanHuyens.Where(x=>x.ma_tinh == idTinh).ToList().Select(e => { e.XaPhuongs = null;e.TinhThanh = null; e.DiaChiKhachHangs = null; return e; }).ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
