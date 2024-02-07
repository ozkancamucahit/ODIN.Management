using Blazored.LocalStorage.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Library.Helpers
{
    public static class Serializations
    {
        public static string SerializeObj<T>(T modelObj) => JsonSerializer.Serialize(modelObj);

        public static T DeserializeJSONString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);

        public static IList<T> DeserializeJSONStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString);
    }
}
