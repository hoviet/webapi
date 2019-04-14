using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class DanhMucSanPhamController : ApiController
    {
        QuanLyBanHangDataContext db = new QuanLyBanHangDataContext();

        [HttpGet]
        [ActionName("danhSach")]
        public IHttpActionResult getSanPhamList()
        {
            try
            {
                List<DanhMucSanPham> listTam = db.DanhMucSanPhams.ToList();
                if (listTam.Count == 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                List<dynamic> obj = new List<dynamic>();
                for(int i = 0; i <listTam.Count; i++)
                {
                    var tam = new
                    {
                        id_danh_muc = listTam[i].id_danh_muc,
                        ten_danh_muc = listTam[i].ten_danh_muc,
                        url_hinh = listTam[i].url_hinh
                    };
                    obj.Add(tam);
                }
                return Ok(obj);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("layMot")]
        public IHttpActionResult MotLoaiSanPham(int id)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == id);
                if(dmsp == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                var tam = new
                {
                    idDanhMuc = dmsp.id_danh_muc,
                    tenDanhMuc = dmsp.ten_danh_muc,
                    urlHinh = dmsp.url_hinh
                };
                return Ok(dmsp);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        // url Hinh = base64
        [HttpPost]
        [ActionName("them")]
        public IHttpActionResult InsertNewDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPham)
        {
            try
            {
                // copy data của hình ảnh. khi 
                // data của ảnh sẽ được xóa trước khi lưu database
                // chỉ lưu lại url của ảnh vào database
                string base64 = danhMucSanPham.url_hinh;
                // kiểm tra thông tin đầu vào
                if (string.IsNullOrEmpty(danhMucSanPham.ten_danh_muc) || string.IsNullOrEmpty(danhMucSanPham.url_hinh))
                {
                    return BadRequest(new ResponseData
                    {
                        ResponseMessage = "Thông tin nhập chưa đúng",
                        ResponseCode = 1
                    }.ToString());
                }
                // kiểm tra danh mục đã tồn tại
                DanhMucSanPham dsp = db.DanhMucSanPhams.FirstOrDefault(e => e.ten_danh_muc.Equals(danhMucSanPham.ten_danh_muc));
                if (dsp != null)
                {
                    // lưu database
                    danhMucSanPham.url_hinh = "";
                    db.DanhMucSanPhams.InsertOnSubmit(danhMucSanPham);
                    db.SubmitChanges();

                    // lưu ảnh vào server
                    byte[] imageBytes = Convert.FromBase64String(base64);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    string fileName = "danh_muc_" + danhMucSanPham.id_danh_muc + ".png";
                    image.Save(Path.Combine(HostingEnvironment.MapPath("~/hinh/DanhMuc"), fileName));

                    // update lại url hình ảnh vào db
                    danhMucSanPham.url_hinh = "/hinh/DanhMuc/" + fileName;
                    db.SubmitChanges();
                }
                return Ok(new ResponseData
                {
                    ResponseCode = 2,
                    ResponseMessage = "Thành công"
                }.ToString());
            }
            catch (Exception ex)
            {
                return Ok(new ResponseData
                {
                    ResponseCode = 1,
                    ResponseMessage = ex.Message
                }.ToString());
            }
        }
        [HttpPut]
        [ActionName("sua")]
        public IHttpActionResult UpdateDanhMucSanPham([FromBody] DanhMucSanPham danhMucSanPhamMoi)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == danhMucSanPhamMoi.id_danh_muc);
                // kiểm tra danh mục có tồn tại
                if (dmsp == null)
                {
                    return Ok(new ResponseData
                    {
                        ResponseCode = 1,
                        ResponseMessage = "Danh mục sản phẩm không tồn tại."
                    }.ToString());
                }
                // kiểm tra thông tin input
                if (danhMucSanPhamMoi.ten_danh_muc == null)
                {
                    return Ok(new ResponseData
                    {
                        ResponseCode = 1,
                        ResponseMessage = "Thông tin nhập chưa đúng."
                    }.ToString());
                }
                // update db
                dmsp.ten_danh_muc = danhMucSanPhamMoi.ten_danh_muc;
                db.SubmitChanges();

                // update hình ảnh
                if (!string.IsNullOrEmpty(danhMucSanPhamMoi.url_hinh))
                {
                    // lưu ảnh vào server
                    byte[] imageBytes = Convert.FromBase64String(danhMucSanPhamMoi.url_hinh);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    string fileName = "danh_muc_" + dmsp.id_danh_muc + ".png";
                    image.Save(Path.Combine(HostingEnvironment.MapPath("~/hinh/DanhMuc"), fileName));
                }

                return Ok(new ResponseData
                {
                    ResponseCode = 2,
                    ResponseMessage = "Thành công"
                }.ToString());
            }
            catch (Exception ex)
            {
                return Ok(new ResponseData
                {
                    ResponseCode = 1,
                    ResponseMessage = ex.Message
                });
            }
        }
        [HttpDelete]
        [ActionName("xoa")]
        public IHttpActionResult DeleteDanhMucSanPham(int id)
        {
            try
            {
                DanhMucSanPham dmsp = db.DanhMucSanPhams.FirstOrDefault(x => x.id_danh_muc == id);

                if (dmsp == null)
                {
                    return Ok(new ResponseData
                    {
                        ResponseCode = 1,
                        ResponseMessage = "Danh mục sản phẩm không tồn tại."
                    }.ToString());
                }
                db.DanhMucSanPhams.DeleteOnSubmit(dmsp);
                db.SubmitChanges();
                return Ok(new ResponseData
                {
                    ResponseCode = 2,
                    ResponseMessage = "Thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseData
                {
                    ResponseCode = 1,
                    ResponseMessage = ex.Message
                });
            }
        }
    }
}
