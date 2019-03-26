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
    public class DanhSachKhuyenMaiController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        [HttpGet]
        [ActionName("getListKhuyenMai")]
        public IHttpActionResult getKhuyenMaiList()
        {
            try
            {
                List<DanhSachKhuyenMai> list = db.DanhSachKhuyenMais.ToList();
                if (list.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].url_hinh = "http://www.3anhem.somee.com" + list[i].url_hinh;
                }
                return Ok(list);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [ActionName("getMotKhuyenMai")]
        public IHttpActionResult GetKhuyenMai(int id)
        {
            try
            {
                DanhSachKhuyenMai khuyenMai = db.DanhSachKhuyenMais.FirstOrDefault(x => x.id_khuyen_mai == id);
                if (khuyenMai == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(khuyenMai);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("suahinh")]
        public IHttpActionResult uphinh([FromBody] DanhSachKhuyenMai khuyenMai)
        {
            try
            {
                DanhSachKhuyenMai km = db.DanhSachKhuyenMais.FirstOrDefault(e => e.id_khuyen_mai == khuyenMai.id_khuyen_mai);

                byte[] imageBytes = Convert.FromBase64String(khuyenMai.url_hinh);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string fileName = "KhuyenMai_" + km.id_khuyen_mai + ".png";
                image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/KhuyenMai"), fileName));

                km.url_hinh = "/hinh/KhuyenMai/" + fileName;
                db.SubmitChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ActionName("them")]
        public IHttpActionResult insertNewKhuyenMai([FromBody]DanhSachKhuyenMai khuyenMai)
        {
            try
            {
                string base64 = khuyenMai.url_hinh;
                khuyenMai.url_hinh = "";
                db.DanhSachKhuyenMais.InsertOnSubmit(khuyenMai);
                db.SubmitChanges();

                byte[] imageBytes = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string fileName = "KhuyenMai_" + khuyenMai.id_khuyen_mai + ".png";
                image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/KhuyenMai"), fileName));

                khuyenMai.url_hinh = "/hinh/KhuyenMai/" + fileName;
                db.SubmitChanges();

                string API_ACCESS_KEY = "AAAARlBUfkw:APA91bEct_WArzMsLAqiKIEMCg9Vd5S6Eq_jcTiDLI2CTrx_t9VQeecPSMakRKUyKNTO4NcqBYppIxXflvQqortZvfKT9eQTbG_zZjztAh17i7JFU2rfyfPlAbBvl2uDr5sqzJ4CYbOy";
                List<FCM> list = db.FCMs.ToList();
                string noidung = khuyenMai.ten_km + ", khuyến mãi lên đến " + khuyenMai.phan_tram_km + "%, từ ngày " + khuyenMai.t_bat_dau.ToShortDateString() + " đến hết ngày " + khuyenMai.t_ket_thuc.ToShortDateString() + ".";
                for(int i = 0; i<list.Count; i++)
                {

                    var fields = new
                    {
                        data = new
                        {
                            body = noidung,
                            title = khuyenMai.ten_km,
                            image = "http://www.3anhem.somee.com" + khuyenMai.url_hinh

                        },
                        to = list[i].token
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
                }
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IHttpActionResult updateKhuyenMai([FromBody] DanhSachKhuyenMai khuyenMai)
        {
            try
            {
                DanhSachKhuyenMai km = db.DanhSachKhuyenMais.FirstOrDefault(x => x.id_khuyen_mai == khuyenMai.id_khuyen_mai);
                if(km == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                if(khuyenMai.ten_km != null)
                {
                    km.ten_km = khuyenMai.ten_km;
                }
                if(khuyenMai.phan_tram_km != 0)
                {
                    km.phan_tram_km = khuyenMai.phan_tram_km;
                }
                if(khuyenMai.t_bat_dau.Equals("0001 - 01 - 01T00: 00:00")){
                    km.t_bat_dau = khuyenMai.t_bat_dau;
                }
                if (khuyenMai.t_ket_thuc.Equals("0001 - 01 - 01T00: 00:00"))
                {
                    km.t_ket_thuc = khuyenMai.t_ket_thuc;
                }

                db.SubmitChanges();
                return Ok(km);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IHttpActionResult deleteKhuyenMai(int id)
        {
            try
            {
                DanhSachKhuyenMai km = db.DanhSachKhuyenMais.FirstOrDefault(x => x.id_khuyen_mai == id);
                if(km == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                db.DanhSachKhuyenMais.DeleteOnSubmit(km);
                db.SubmitChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
