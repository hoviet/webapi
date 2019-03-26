using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace WebApp.Models
{
    public class ThongBaoChuyenTT
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();
        public void giuThongBao(int idKhachHang, int idTinhTrang )
        {
            List<FCM> fcm = db.FCMs.Where(e => e.khach_hang == idKhachHang).ToList();
            if (fcm.Count != 0)
            {
                TinhTrangDonHang tr = db.TinhTrangDonHangs.FirstOrDefault(e => e.id_tinh_trang == idTinhTrang);

                string API_ACCESS_KEY = "AAAARlBUfkw:APA91bEct_WArzMsLAqiKIEMCg9Vd5S6Eq_jcTiDLI2CTrx_t9VQeecPSMakRKUyKNTO4NcqBYppIxXflvQqortZvfKT9eQTbG_zZjztAh17i7JFU2rfyfPlAbBvl2uDr5sqzJ4CYbOy";
                for (int i = 0; i < fcm.Count; i++)
                {
                    var notification = new
                    {
                        body = tr.tinh_trang_don_hang,
                        title = "TRẠNG THÁI ĐƠN HÀNG",
                        vibrate = 1,
                        sound = "default",
                        click_action = "FCM_PLUGIN_ACTIVITY"
                    };
                    var data = new
                    {
                        message = tr.tinh_trang_don_hang,
                        title = "TRẠNG THÁI ĐƠN HÀNG",

                    };

                    var fields = new
                    {
                        to = fcm[i].token,
                        data = data,
                        notification = notification,
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
            }
            return;
            
        }
    }
}