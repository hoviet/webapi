using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class TinhController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("lay")]
        public IHttpActionResult layTinh()
        {
            try
            {
                List<TinhThanh> list = db.TinhThanhs.ToList().Select(e => { e.QuanHuyens = null; return e; }).ToList();
                if(list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
