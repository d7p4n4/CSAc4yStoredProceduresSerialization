using d7p4n4Namespace.Final.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSAc4yStoredProceduresSerialization
{
    public class Serializer
    {
        public string serialize(Object taroltEljaras, Type anyType, string path)
        {
            XmlSerializer serializer = new XmlSerializer(anyType);
            var xml = "";

            using (var writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                {
                    serializer.Serialize(writer, taroltEljaras);
                    xml = writer.ToString(); // Your XML
                }
            }
            System.IO.File.WriteAllText(path, xml);

            return xml;
        }
    }
}
