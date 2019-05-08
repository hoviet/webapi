using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("DangNhap")]
        public IHttpActionResult dangNhap([FromBody] DN admin)
        {
            try
            {
                Admin ad = db.Admins.FirstOrDefault(e => e.user.Equals(admin.user) && e.pass.Equals(admin.pass));
                if(ad== null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                var tam = new
                {
                    user = ad.user,
                    id = ad.id,
                };
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
