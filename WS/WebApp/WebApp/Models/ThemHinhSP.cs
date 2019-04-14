using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ThemHinhSP
    {
        public string themHinh(string base64, int id)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            string fileName = "san_pham_" + id + ".png";
            image.Save(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/hinh/SanPham"), fileName));

            return fileName;
        }
    }
}