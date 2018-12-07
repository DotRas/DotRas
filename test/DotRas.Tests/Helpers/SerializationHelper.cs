using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotRas.Tests.Helpers
{
    public class SerializationHelper
    {
        public static string Serialize<T>(T value, bool useJson)
        {
            using (var ms = new MemoryStream())
            {
                XmlObjectSerializer serializer;
                if (useJson)
                {
                    serializer = new DataContractJsonSerializer(typeof(T));
                }
                else
                {
                    serializer = new DataContractSerializer(typeof(T));
                }

                serializer.WriteObject(ms, value);

                var bytes = ms.ToArray();

                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
        }
    }
}