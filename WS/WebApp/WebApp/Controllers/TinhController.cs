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
   // [Authorize]
    public class TinhController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("lay")]
        public IHttpActionResult layTinh()
        {
            try
            {
                List<TinhThanh> list = db.TinhThanhs.ToList();
                if(list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> ds = new List<dynamic>();
                for(int i = 0; i< list.Count; i++)
                {
                    var tam = new
                    {
                        ma_tinh = list[i].ma_tinh,
                        ten = list[i].ten,
                        loai = list[i].loai
                    };
                    ds.Add(tam);
                }
                return Ok(ds);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
