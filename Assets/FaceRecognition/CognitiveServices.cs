using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CognitiveServices : MonoBehaviour {

    public IEnumerator<object> PostToFace(byte[] imageData, string type) {
        //Parameters
        bool returnFaceId= true;
        string[] faceAttributes = new string[] { "age", "gender", "emotion" };

        var url = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/{0}?returnFaceId={1}&returnFaceAttributes={2}", type, returnFaceId, Converters.ConvertStringArrayToString(faceAttributes));
        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", Constants.MCS_FACEKEY },
            { "Content-Type", "application/octet-stream" }
        };

        WWW www = new WWW(url, imageData, headers);
        yield return www;

        //Json Response
        JSONObject j = new JSONObject(www.text);

        Debug.Log(j);

        if (j != null)
            SaveJsonToFaceModel(j);
    }

    private void SaveJsonToFaceModel(JSONObject j)
    {
        FaceObject faceObj = new FaceObject();
        List<Face> faces = new List<Face>();

        foreach (var faceItem in j.list)
        {
            Face face = new Face();

            face = new Face() { faceId = faceItem.GetField("faceId").ToString() };

            var faceRectangle = faceItem.GetField("faceRectangle");
            face.faceRectangle = new FaceRectangle()
            {
                left = int.Parse(faceRectangle.GetField("left").ToString()),
                top = int.Parse(faceRectangle.GetField("top").ToString()),
                width = int.Parse(faceRectangle.GetField("width").ToString()),
                height = int.Parse(faceRectangle.GetField("height").ToString())
            };

            var faceAttributes = faceItem.GetField("faceAttributes");
            face.faceAttributes = new FaceAttributes()
            {
                age = int.Parse(faceAttributes.GetField("age").ToString().Split('.')[0]),
                gender = faceAttributes.GetField("gender").ToString().Replace("\"", "") == "male" ? 0 : 1
            };

            var emotion = faceAttributes.GetField("emotion");
            face.emotionAttributes = new EmotionAttributes()
            {
                anger = float.Parse(emotion.GetField("anger").ToString()),
                contempt = float.Parse(emotion.GetField("contempt").ToString()),
                disgust = float.Parse(emotion.GetField("disgust").ToString()),
                fear = float.Parse(emotion.GetField("fear").ToString()),
                happiness = float.Parse(emotion.GetField("happiness").ToString()),
                neutral = float.Parse(emotion.GetField("neutral").ToString()),
                sadness = float.Parse(emotion.GetField("sadness").ToString()),
                surprise = float.Parse(emotion.GetField("surprise").ToString()),
            };
            faces.Add(face);
        }

        faceObj.faces = faces;

        PlayVoiceMessage.Instance.PlayTextToSpeechMessage(faceObj);
    }
}
