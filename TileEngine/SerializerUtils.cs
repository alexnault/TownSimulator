using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TileEngine
{
    public static class SerializerUtils
    {
        public static T Deserialize<T>(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                var xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(stream);
            }
        }
        public static string Serialize<T>(T obj)
        {
            return Serialize<T>(obj, string.Empty, string.Empty);
        }
        public static string Serialize<T>(T obj, string nsPrefix, string ns)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    var xs = new XmlSerializer(typeof(T));
                    var xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(nsPrefix, ns);
                    xs.Serialize(sw, obj, xmlns);
                    return Encoding.UTF8.GetString(ms.ToArray()).Trim();
                }
            }
        }

    }
}
