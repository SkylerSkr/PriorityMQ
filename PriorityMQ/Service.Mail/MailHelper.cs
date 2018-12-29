using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mail
{
    public static class MailHelper
    {
        public static bool SendMail(MyMail myMail)
        {
            var senduser = System.Configuration.ConfigurationSettings.AppSettings["senduser"];//
            var sendpwd = System.Configuration.ConfigurationSettings.AppSettings["sendpwd"];// "test";

            var message = new MailMessage();
            var fromddress = new MailAddress(senduser);
            message.From = fromddress;

            foreach (var item in myMail.Receiver)
            {
                message.To.Add(item);//接收方
            }


            if (myMail.CC != null)
            {
                foreach (var item in myMail.CC)
                {
                    message.CC.Add(item);//抄送方
                }
            }

            message.Subject = myMail.Subject;//主题
            message.Body = myMail.Body;//内容

            if (myMail.AttachmentsPath != null)
            {
                foreach (var item in myMail.AttachmentsPath)
                {
                    WebRequest request = WebRequest.Create(item.Value);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    message.Attachments.Add(new Attachment(responseStream, item.Key));//附件
                }
            }

            SmtpClient client = new SmtpClient("smtp.mxhichina.com", 25);//基于qq邮件发送
            client.Credentials = new NetworkCredential(senduser, sendpwd);
            client.EnableSsl = true;
            client.Send(message);
            return true;
        }
    }
}
