using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class FCMController : ApiController
    {
        private QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("kiemTra")]
        public IHttpActionResult kiemtra(string device, string token)
        {
            try
            {
                FCM fcm = db.FCMs.FirstOrDefault(e => e.device.Equals(device) && e.token.Equals(token));

                if (fcm == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                return Ok(fcm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("TaoMoi")]
        public IHttpActionResult taoMoi(string device, string token)
        {
            try
            {
                FCM fcm = db.FCMs.FirstOrDefault(e => e.device.Equals(device) && e.token.Equals(token));

                if (fcm == null)
                {
                    FCM tam = new FCM();
                    tam.device = device;
                    tam.token = token;

                    db.FCMs.InsertOnSubmit(tam);
                    db.SubmitChanges();

                    return Ok(tam);
                }
                return Ok(fcm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        [HttpGet]
        [ActionName("DangXuat")]
        public IHttpActionResult DangXuatFCM(int id)
        {
            try
            {
                FCM fcm = db.FCMs.FirstOrDefault(e => e.khach_hang == id);
                if (fcm == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                fcm.khach_hang = 0;
                db.SubmitChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("DanhSach")]
        public IHttpActionResult danhSach()
        {
            try
            {
                List<FCM> list = db.FCMs.ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [ActionName("Xoa")]
        public IHttpActionResult Xoa(int id)
        {
            try
            {
                FCM fcm = db.FCMs.FirstOrDefault(e => e.id == id);
                if (fcm == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.FCMs.DeleteOnSubmit(fcm);
                db.SubmitChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[ActionName("thongbao")]
        //public IHttpActionResult Push([FromBody] FCMInfo data)
        //{
        //    try
        //    {
        //        string API_ACCESS_KEY = "AAAARlBUfkw:APA91bEct_WArzMsLAqiKIEMCg9Vd5S6Eq_jcTiDLI2CTrx_t9VQeecPSMakRKUyKNTO4NcqBYppIxXflvQqortZvfKT9eQTbG_zZjztAh17i7JFU2rfyfPlAbBvl2uDr5sqzJ4CYbOy";
        //        for (int i = 0; i < data.to.Count; i++)
        //        {
        //            var fields = new
        //            {
        //                data = new
        //                {
        //                    body = data.body,
        //                    title = data.title,
        //                    image = data.image

        //                },
        //                to = data.to[i]
        //            };

        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //            request.Method = "POST";
        //            request.Headers.Add("Authorization", "key=" + API_ACCESS_KEY);

        //            request.ContentType = "application/json";

        //            string postData = new JavaScriptSerializer().Serialize(fields);

        //            byte[] bytes = Encoding.UTF8.GetBytes(postData);
        //            request.ContentLength = bytes.Length;

        //            Stream requestStream = request.GetRequestStream();
        //            requestStream.Write(bytes, 0, bytes.Length);

        //            WebResponse response = request.GetResponse();
        //            Stream stream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(stream);

        //            var result = reader.ReadToEnd();
        //            stream.Dispose();
        //            reader.Dispose();
        //        }
        //        return Ok();
        //    }catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost]
        //[ActionName("quangcao")]
        //public IHttpActionResult quangCaoAll([FromBody] FCMQuangCao data)
        //{
        //    try
        //    {
        //        string API_ACCESS_KEY = "AAAARlBUfkw:APA91bEct_WArzMsLAqiKIEMCg9Vd5S6Eq_jcTiDLI2CTrx_t9VQeecPSMakRKUyKNTO4NcqBYppIxXflvQqortZvfKT9eQTbG_zZjztAh17i7JFU2rfyfPlAbBvl2uDr5sqzJ4CYbOy";

        //        List<FCM> list = db.FCMs.Where(e => e.khach_hang == data.idKhachHang).ToList();
        //        if (list.Count == 0)
        //        {
        //            return StatusCode(HttpStatusCode.NoContent);
        //        }

        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            var fields = new
        //            {
        //                data = new
        //                {
        //                    body = data.body,
        //                    title = data.title,
        //                    image = data.image

        //                },
        //                to = list[i].token
        //            };

        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //            request.Method = "POST";
        //            request.Headers.Add("Authorization", "key=" + API_ACCESS_KEY);

        //            request.ContentType = "application/json";

        //            string postData = new JavaScriptSerializer().Serialize(fields);

        //            byte[] bytes = Encoding.UTF8.GetBytes(postData);
        //            request.ContentLength = bytes.Length;

        //            Stream requestStream = request.GetRequestStream();
        //            requestStream.Write(bytes, 0, bytes.Length);

        //            WebResponse response = request.GetResponse();
        //            Stream stream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(stream);

        //            var result = reader.ReadToEnd();
        //            stream.Dispose();
        //            reader.Dispose();
        //        }
        //        return Ok();
        //    }catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}     
    }
}
