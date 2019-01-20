using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CognitiveServices : MonoBehaviour {
    public IEnumerator<object> PostToFace(byte[] imageData) {
        //Parameters
        bool returnFaceId= true;
        string[] faceAttributes = new string[] { "age", "gender", "emotion" };

        var url = string.Format("https://westus.api.cognitive.microsoft.com/face/v1.0/{0}?returnFaceId={1}&returnFaceAttributes={2}", "detect", returnFaceId, Converters.ConvertStringArrayToString(faceAttributes));
        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", Constants.MCS_FACEKEY },
            { "Content-Type", "application/octet-stream" }
        };

        WWW www = new WWW(url, imageData, headers);
        yield return www;

        if (www.error == null) {
            Debug.Log("Cognitive, Response: " + www.text);

            JSONObject j = new JSONObject(www.text);

            if (j != null) {
                SaveJsonToFaceModel(j);
            }
        } else {
            Debug.Log("WWW Error: " + www.error + " Response: " + www.text);
        }
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

  public IEnumerator<object> PostToOCR(byte[] imageData)
  {
    var url = string.Format("https://eastus2.api.cognitive.microsoft.com/vision/v2.0/ocr?detectOrientation=true");
    var headers = new Dictionary<string, string>() {
        { "Ocp-Apim-Subscription-Key", Constants.MCS_VISIONKEY },
        { "Content-Type", "application/octet-stream" }
    };

    WWW www = new WWW(url, imageData, headers);
    yield return www;

    if (www.error == null)
    {
      Debug.Log("Cognitive, Response: " + www.text);

      JSONObject j = new JSONObject(www.text);

      HandleOcrJsonResponse(j);
    }
    else
    {
      Debug.Log("WWW Error: " + www.error + " Response: " + www.text);
    }
  }

  private void HandleOcrJsonResponse(JSONObject j)
  {
        List<string> wordList = new List<string>();
        string textAngle = string.Empty;

        try { textAngle = j.GetField("textAngle").ToString(); }
        catch { }

        //Add computerVisionOCRDto
        OCRObject computerVisionOCR = new OCRObject()
        {
            language = j.GetField("language").ToString(),
            textAngle = textAngle,
            orientation = j.GetField("orientation").ToString()
        };

        //Add region, words
        var region = j.GetField("regions");
        if (region.list.Count != 0)
        {
        foreach (var regionItem in region.list)
        {
            var lines = regionItem.GetField("lines");
            foreach (var line in lines.list)
            {
            var words = line.GetField("words");
            foreach (var word in words.list)
            {
                wordList.Add(word.GetField("text").ToString().Replace("\"", ""));
            }
            }
        }
        }

        computerVisionOCR.text = string.Join(" ", wordList.ToArray());

        if (wordList.Count > 0) {
            Globals.instance.textToSpeech.StartSpeaking(computerVisionOCR.text);

            Globals.instance.textCycler.strings.AddRange(wordList);
        } else {
            Globals.instance.textToSpeech.StartSpeaking("I wasn't able to read anything");
        }
  }
}
