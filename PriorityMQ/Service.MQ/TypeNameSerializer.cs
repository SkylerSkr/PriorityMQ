using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MQ
{
    internal class TypeNameSerializer : ITypeNameSerializer
    {
        Type ITypeNameSerializer.DeSerialize(string typeName)
        {
            throw new NotImplementedException();
        }

        string ITypeNameSerializer.Serialize(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
