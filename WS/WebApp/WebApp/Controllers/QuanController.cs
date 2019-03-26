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
                List<QuanHuyen> list = db.QuanHuyens.Where(x=>x.ma_tinh == idTinh).ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> ds = new List<dynamic>();
                for (int i = 0; i < list.Count; i++)
                {
                    var tam = new
                    {
                        ma_tinh = list[i].ma_tinh,
                        ma_quan_huyen = list[i].ma_quan_huyen,
                        ten_quan_huyen = list[i].ten_quan_huyen,
                        loai = list[i].loai
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
