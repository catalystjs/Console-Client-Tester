using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Console.Client.Handlers
{
    public static class JsonConvert
    {
        public static string SerializeObject(object value)
        {
            using (var stream = new MemoryStream())
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;

                var serializer = new DataContractJsonSerializer(value.GetType(),settings);
                //var serializer = new DataContractJsonSerializer(value.GetType());

                serializer.WriteObject(stream, value);
                byte[] json = stream.ToArray();
                stream.Close();

                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        public static T DeserializeObject<T>(string value) where T : class
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var output = serializer.ReadObject(stream) as T;
                stream.Close();

                return output;
            }
        }
    }
}
