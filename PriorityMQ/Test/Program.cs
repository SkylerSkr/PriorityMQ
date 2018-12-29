using Newtonsoft.Json;
using Service.Mail;
using Service.MQ;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0, 50, i =>
            {
                //发送优先级邮件到fanout_Mail交换机，routingkey为mail
                MQHelperFactory.Default().FanoutPush<MyMail>(new MyMail
                {
                    Receiver = new List<string>() { "tangoshi@humanbacker.com" },
                    Body = i + "Body",
                    Subject = "Test"
                }, out string msg, "fanout_Mail", "mail", priority: i); //设置优先级});
            });

            //拉取fanout_queue_Mail队列的消息，并绑定到fanout_Mail交换机，routingkey为mail
            MQHelperFactory.Default().FanoutConsume(item =>
            {
                var mail = JsonConvert.DeserializeObject<MyMail>(item.Item2);
                Console.WriteLine(mail.Body);
                MailHelper.SendMail(mail);
            }, "fanout_Mail", "fanout_queue_Mail", "mail", maxPriority: 100);

            Console.ReadKey();
        }
    }
}
