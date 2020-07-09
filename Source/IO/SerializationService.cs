using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BridgeManager.Source.IO
{
    public class SerializationService
    {
        XmlSerializer xmlSerializer;

        public SerializationService()
        {
            xmlSerializer = new XmlSerializer(typeof(Tournament));
        }

        public void Serialize(Tournament tournament, string filename)
        {
            using (TextWriter writer = new StreamWriter(filename))
            {
                xmlSerializer.Serialize(writer, tournament);
            }
        }

        public Tournament Deserialize(string filepath)
        {
            using (var stream = new StreamReader(filepath))
            using (var reader = new XmlTextReader(stream))
            {
                if (xmlSerializer.CanDeserialize(reader))
                {
                    try
                    {
                        return xmlSerializer.Deserialize(reader) as Tournament;
                    }
                    catch(Exception) { throw; }
                }
                else throw new Exception("File unsuitable for deserialization");

            }
        }

    }
}
