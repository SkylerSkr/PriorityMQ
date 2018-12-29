using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mail
{
    public class MyMail
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public List<string> Receiver { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        public List<string> CC { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public Dictionary<string, string> AttachmentsPath { get; set; }
    }
}
