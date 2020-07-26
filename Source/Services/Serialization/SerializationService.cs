using BridgeManager.Source.Model;
using BridgeManager.Source.Services.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BridgeManager.Source.IO
{
    public class SerializationService : ISerializationService
    {

        private JsonSerializer serializer;

        public SerializationService()
        {
            serializer = new JsonSerializer();
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
        }

        public void Serialize(Tournament tournament, string filepath)
        {

            using (StreamWriter sw = new StreamWriter(filepath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, tournament);
            }
        }

        public Tournament Deserialize(string filepath)
        { 
            try
            {
                using (StreamReader reader = File.OpenText(filepath))
                {
                   return serializer.Deserialize(reader, typeof(Tournament)) as Tournament;
                }
            }
            catch (Exception) { throw; }
        }

    }
}
