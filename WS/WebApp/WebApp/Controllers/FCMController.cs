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

                if(fcm == null)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
                return Ok(fcm);
            }catch(Exception ex)
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
                if(list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }

                return Ok(list);
            }catch(Exception ex)
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

        [HttpPost]
        [ActionName("thongbao")]
        public IHttpActionResult Push(string to)
        {
            string API_ACCESS_KEY = "AAAARlBUfkw:APA91bEct_WArzMsLAqiKIEMCg9Vd5S6Eq_jcTiDLI2CTrx_t9VQeecPSMakRKUyKNTO4NcqBYppIxXflvQqortZvfKT9eQTbG_zZjztAh17i7JFU2rfyfPlAbBvl2uDr5sqzJ4CYbOy";
            var notification = new
            {
                body = "test",
                title = "test",
                vibrate = 1,
                sound = "default",
                click_action = "FCM_PLUGIN_ACTIVITY"
            };
            var data = new
            {
                message = "test",
                title = "test"

            };
                var fields = new
                {
                    data = new
                    {
                        body = "Body of Your Notification in Data",
                        title = "Title of Your Notification in Title",
                        image = "https://vcdn.tikicdn.com/media/upload/landingpage/banners/3189279a60f7f26e9c750ee285d1ee76.png"

                    },
                    to = "cDGZiMq4eqg:APA91bFKR_P5H-9luwNs1ig77vA4UHichUswRm3MhgOSwnSLipc0sqEUmiEOIQKdD-YmolBuwzr_mlLYe2UnUk6asBRixXTyNMgZ4GW-QCgwm8RJT9YwaKPWt-eMr6oPTp1AdrURhgFS"
                };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.Headers.Add("Authorization", "key=" + API_ACCESS_KEY);

            request.ContentType = "application/json";

            string postData = new JavaScriptSerializer().Serialize(fields);

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();

            return  Ok(result);
        }

        //[HttpPost]
        //[ActionName("thongbao")]
        //public IHttpActionResult guiTHongBao(string to)
        //{
        //    string API_ACCESS_KEY = "AAAARlBUfkw:APA91bEct_WArzMsLAqiKIEMCg9Vd5S6Eq_jcTiDLI2CTrx_t9VQeecPSMakRKUyKNTO4NcqBYppIxXflvQqortZvfKT9eQTbG_zZjztAh17i7JFU2rfyfPlAbBvl2uDr5sqzJ4CYbOy";
        //    var notification = new
        //    {
        //        body = "test",
        //        title = "test",
        //        vibrate = 1,
        //        sound = "default",
        //        click_action = "FCM_PLUGIN_ACTIVITY"
        //    };

        //    var data = new
        //    {
        //        message = "test",
        //        title = "test"

        //    };

        //    var fields = new
        //    {
        //        to = to,
        //        data = data,
        //        notification = notification,
        //    };

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //    request.Method = "POST";
        //    request.Headers.Add("Authorization", "key=" + API_ACCESS_KEY);

        //    request.ContentType = "application/json";

        //    string postData = new JavaScriptSerializer().Serialize(fields);

        //    byte[] bytes = Encoding.UTF8.GetBytes(postData);
        //    request.ContentLength = bytes.Length;

        //    Stream requestStream = request.GetRequestStream();
        //    requestStream.Write(bytes, 0, bytes.Length);

        //    WebResponse response = request.GetResponse();
        //    Stream stream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(stream);

        //    var result = reader.ReadToEnd();
        //    stream.Dispose();
        //    reader.Dispose();

        //    return Ok(result);
        //}
    }
}
