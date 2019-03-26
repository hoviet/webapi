using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace WebApp.Models
{
    public class guiEmail
    {
        public void SendGmail(string toEmail, string toName, string mailBody)
        {
            string fromEmail = "vtnshop@3anhem.somee.com";
            string password = "matkhaula123";
            string fromName = "Admin";
            string mailSubject = "Xác Nhận Tài Khoản";
            var fromAddress = new MailAddress(fromEmail, fromName);
            var toAddress = new MailAddress(toEmail, toName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password),
                Timeout = 20000,

            };

            var message = new MailMessage(fromAddress, toAddress);
            message.IsBodyHtml = true;
            message.Subject = mailSubject;
            message.Body = mailBody;
            smtp.Send(message);
        }
        public string maimailBody(string email, string matKhau)
        {
            string link = "http://www.3anhem.somee.com/api/KhachHang/xacnham?taiKhoan=" + email + "&&matKhau=" + matKhau;
            string tam = "<head> <title>Udacity_email</title> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"> <meta content=\"width=device-width, initial-scale=1.0\" name=\"viewport\"> <!--[if !mso]><!-- --><meta content=\"IE=edge\" http-equiv=\"X-UA-Compatible\"><!--<![endif]--> <!--[if !mso]><!-- --><link href=\"https://fonts.googleapis.com/css?family=Open+Sans:600,400,300\" rel=\"stylesheet\" type=\"text/css\"><!--<![endif]--> <style type=\"text/css\">html, body { background-color:#fafbfc; } img { display:block; } .ReadMsgBody {width: 100%; } .ExternalClass {width: 100%; } * { -webkit-text-size-adjust: none; } .whiteLinks a:link, .whiteLinks a:visited { color:#ffffff!important;} .appleLinksGrey a { color:#b7bdc1!important; text-decoration:none!important; } table {border-collapse:collapse;} .preheader{ font-size: 1px; line-height:1px; display: none!important; mso-hide:all; } #maincontent td{color:#525C65;} </style> </head> <body bgcolor=\"#fafbfc\" style=\"Margin:0; padding:0;\" yahoo=\"fix\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody><tr> <td style=\"background-color:#fafbfc\"> <center bgcolor=\"#fafbfc\" style=\"width:100%;background-color:#fafbfc;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;\"> <div id=\"maincontent\" style=\"max-width:620px; font-size:0;margin:0 auto;\"> <div class=\"preheader\" style=\"font-size: 1px; line-height:1px; display: none!important; mso-hide:all;\"> One more step to get started </div> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;\"> <tbody><tr> <td> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;\"> <tbody><tr> <td align=\"center\" style=\"padding-bottom:20px;\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:'Open+Sans', 'Open Sans', Helvetica, Arial, sans-serif; font-size:13px; line-height:18px; color:#00C0EA; text-align:center; width:152px;\"> <tbody><tr> <td style=\"padding:20px 0 10px 0;\"> <a href=\"#tba\" style=\"text-decoration:none;\" target=\"_blank\"><img alt=\"Udacity\" border=\"0\" height=\"27\" src=\"https://salt.tikicdn.com/cache/w750/ts/banner/33/c6/5d/ed0648ae67b0024e4a1258e3fafebbc5.jpg\" style=\"display:block; width:152px !important; font-family:'Open+Sans', 'Open Sans', Helvetica, Arial, sans-serif; font-size:22px; line-height:26px; color:#000000; text-transform:uppercase; text-align:center; letter-spacing:1px;\" width=\"152\"></a> </td> </tr> </tbody></table> </td> </tr> </tbody></table> </td> </tr> <tr> <td> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;\"> <tbody><tr> <td bgcolor=\"#fafbfc\" style=\"width:7px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#f5f6f7\" style=\"width:1px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#f0f2f3\" style=\"width:1px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#edeef1\" style=\"width:1px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#ffffff\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;\"> <tbody><tr> <td style=\"text-align:center; padding:40px 40px 40px 40px; border-top:3px solid #02b3e4;\"> <div style=\"display:inline-block; width:100%; max-width:520px;\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:'Open+Sans', 'Open Sans', Helvetica, Arial, sans-serif; font-size:14px; line-height:24px; color:#525C65; text-align:left; width:100%;\"> <tbody><tr> <td> <p style=\"Margin:0; font-size:18px; line-height:23px; color:#102231; font-weight:bold;\"> <strong> Chào Bạn,</strong><br><br> </p> </td> </tr> <tr> <td> Để hoàn tất đăng ký, vui lòng xác minh email của bạn: <br><br> </td> </tr> <tr> <td align=\"center\" style=\"padding:15px 0 40px 0; border-bottom:1px solid #f3f6f9; \"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate !important; border-radius:15px; width:210px;\"> <tbody><tr> <td align=\"center\" valign=\"top\"> <a href=\""+link+ "\"target=\"_blank\" style=\"background-color:#01b3e3; border-collapse:separate !important; border-top:10px solid #01b3e3; border-bottom:10px solid #01b3e3; border-right:45px solid #01b3e3; border-left:45px solid #01b3e3; border-radius:4px; color:#ffffff; display:inline-block; font-family:'Open+Sans','Open Sans',Helvetica, Arial, sans-serif; font-size:13px; font-weight:bold; text-align:center; text-decoration:none; letter-spacing:2px;\">XÁC NHẬN</a> </td> </tr> </tbody></table> </td> </tr> <tr> <td style=\"padding-top:30px;\"> <p style=\"Margin:20px 0 20px 0;\">Hoặc sao chép liên kết này và dán vào trình duyệt web của bạn</p> <p style=\"Margin:20px 0; font-size:12px; line-height:17px; word-wrap:break-word; word-break:break-all;\"><a href=\""+link+"\" style=\"color:#5885ff; text-decoration:underline;\" target=\"_blank\">"+link+"\"</a></p> </td> </tr> <tr> <td style=\"font:bold 14px/16px Arial, Helvetica, sans-serif; color:#363636; padding:0 0 7px;\"> Cảm ơn bạn. </td> </tr> </tbody></table> </div> </td> </tr> <tr> <td bgcolor=\"#e0e2e5\" style=\"height:1px; width:100%; line-height:1px; font-size:0;\">&nbsp;</td> </tr> <tr> <td bgcolor=\"#e0e2e4\" style=\"height:1px; width:100%; line-height:1px; font-size:0;\">&nbsp;</td> </tr> <tr> <td bgcolor=\"#e8ebed\" style=\"height:1px; width:100%; line-height:1px; font-size:0;\">&nbsp;</td> </tr> <tr> <td bgcolor=\"#f1f3f6\" style=\"height:1px; width:100%; line-height:1px; font-size:0;\">&nbsp;</td> </tr> </tbody></table> </td> <td bgcolor=\"#edeef1\" style=\"width:1px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#f0f2f3\" style=\"width:1px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#f5f6f7\" style=\"width:1px; font-size:1px;\">&nbsp;</td> <td bgcolor=\"#fafbfc\" style=\"width:7px; font-size:1px;\">&nbsp;</td> </tr> </tbody></table> </td> </tr> </body";
            return tam;
        }
    }
}