using Newtonsoft.Json;
using System.IO;

namespace FlockLibrary
{
    public class FlockSerializer
    {
        private static FlockSerializer instance;

        private FlockSerializer()
        {  
        }

        public static FlockSerializer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlockSerializer();

                }
                return instance;
            }
        }

        public Flock JsonToFlock(string json)
        {
            return JsonConvert.DeserializeObject<Flock>(json);
        }

        public Flock StreamToFlock(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                return JsonToFlock(json);
            }
        }

        public void FlockToFile(Flock flock, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                var json = FlockToJson(flock);
                writer.Write(json);
            }
        }

        public Flock FileToFlock(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                return StreamToFlock(stream);
            }
        }

        public string FlockToJson(Flock flock)
        {
            return JsonConvert.SerializeObject(flock, Formatting.Indented);
        }

    }
}
