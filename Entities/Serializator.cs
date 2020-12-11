using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Entities
{
    public static class Serializator
    {
        public static byte[] Serialization(object objToSerialize)
        {
            if (objToSerialize == null)
            {
                return null;
            }

            try
            {
                using MemoryStream stream = new MemoryStream();
                Serializer.Serialize(stream, objToSerialize);
                return stream.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static object Deserialization(byte[] data, Type type)
        {
            if (data == null)
            {
                return null;
            }

            try
            {
                using MemoryStream stream = new MemoryStream(data);
                var result = Serializer.Deserialize(type, stream);
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
