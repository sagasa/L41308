using System;
using System.IO;
using Newtonsoft.Json;

namespace Giraffe.Saves
{
    public static class SaveManager
    {
        private static JsonSerializer jsonSerializer = new JsonSerializer();

        public static void Save<Type>(string name,Type data)
        {
            using (var fs = new FileStream("./"+name+".json", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            using (var writer = new JsonTextWriter(sw))
            {
                jsonSerializer.Serialize(writer,data);
            }
        }

        public static Type Load<Type>(string name)
        {
            try
            {
                using (var fs = new FileStream("./" + name + ".json", FileMode.Open))
                using (var sr = new StreamReader(fs))
                using (var reader = new JsonTextReader(sr))
                {
                    return jsonSerializer.Deserialize<Type>(reader);
                }
            }
            catch (FileNotFoundException e)
            {
                return default(Type);
            }
        }
    }
}