using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class XaController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("lay")]
        public IHttpActionResult layXa(string idQuan)
        {
            try
            {
                List<XaPhuong> list = db.XaPhuongs.Where(x => x.ma_quan_huyen == idQuan).ToList().Select(e => { e.QuanHuyen = null;  return e; }).ToList();
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
