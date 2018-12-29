using EasyNetQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MQ
{
    /// <summary>
    /// 消息服务器连接器
    /// </summary>
    public class BusBuilder
    {
        private string _connectionString { get; set; }
        private string _connectionConfigName { get; set; }

        public BusBuilder(string host)
        {
            _connectionConfigName = $"rabbitmq-{host}";
            _connectionString = ConfigurationManager.AppSettings[_connectionConfigName] ?? "";
        }

        public IBus CreateMessageBus()
        {
            // 消息服务器连接字符串
            if (_connectionString == null || _connectionString == string.Empty)
                throw new Exception(
                    $"config[{_connectionConfigName}] not exist,  messageserver connection string is missing or empty");
            return RabbitHutch.CreateBus(_connectionString, serviceRegister => serviceRegister.Register<ISerializer>(
                serviceProvider => new MyJsonSerializer(new TypeNameSerializer())));
        }
    }
    public class MyJsonSerializer : ISerializer
    {
        private readonly ITypeNameSerializer typeNameSerializer;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        public MyJsonSerializer(ITypeNameSerializer typeNameSerializer)
        {
            this.typeNameSerializer = typeNameSerializer;
        }
        public byte[] MessageToBytes<T>(T message) where T : class
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, serializerSettings));
        }
        public T BytesToMessage<T>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), serializerSettings);
        }
        public object BytesToMessage(Type typeName, byte[] bytes)
        {
            var type = typeNameSerializer.DeSerialize(typeName.ToString());
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), type, serializerSettings);
        }


    }
}
