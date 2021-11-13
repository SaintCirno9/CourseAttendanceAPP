using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppShared.Services
{
    public class FaceRecognitionService
    {
        private Baidu.Aip.Face.Face FaceClient { get; set; }

        public FaceRecognitionService(string appId, string apiKey, string secretKey)
        {
            FaceClient = new Baidu.Aip.Face.Face(apiKey, secretKey) {Timeout = 60000};
        }

        public string GetFaceMatchJsonResult(string imgString1, string imgString2)
        {
            var faces = new JArray
            {
                new JObject
                {
                    {"image", imgString1},
                    {"image_type", "BASE64"},
                    {"face_type", "LIVE"},
                    {"quality_control", "LOW"},
                    {"liveness_control", "NONE"},
                },
                new JObject
                {
                    {"image", imgString2},
                    {"image_type", "BASE64"},
                    {"face_type", "LIVE"},
                    {"quality_control", "LOW"},
                    {"liveness_control", "NONE"},
                }
            };
            return JsonConvert.SerializeObject(FaceClient.Match(faces));
        }
    }
}