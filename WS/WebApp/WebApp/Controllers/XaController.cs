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
    public class XaController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("lay")]
        public IHttpActionResult layXa(string idQuan)
        {
            try
            {
                List<XaPhuong> list = db.XaPhuongs.Where(x => x.ma_quan_huyen == idQuan).ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> ds = new List<dynamic>();
                for (int i = 0; i < list.Count; i++)
                {
                    var tam = new
                    {
                        ma_xa_phuong = list[i].ma_xa_phuong,
                        ten = list[i].ten,
                        loai = list[i].loai,
                        ma_quan_huyen = list[i].ma_quan_huyen
                    };
                    ds.Add(tam);
                }
                return Ok(ds);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
