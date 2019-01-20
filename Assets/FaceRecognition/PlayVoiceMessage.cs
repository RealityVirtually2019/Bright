using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayVoiceMessage : MonoBehaviour {

    public static PlayVoiceMessage Instance { get; private set; }

    public TextToSpeech ttsManager;

    void Awake()
    {
        Instance = this;
    }

    public void PlayTextToSpeechMessage(FaceObject faceObj)
    {
        string message = string.Empty;
        string emotionName = string.Empty;

        if (faceObj.faces.Count > 0)
        {
            foreach (Face face in faceObj.faces) {
                EmotionAttributes emotionAttributes = face.emotionAttributes;

                Dictionary<string, float> emotions = new Dictionary<string, float>
                {
                    { "neutral", emotionAttributes.neutral },
                    { "anger", emotionAttributes.anger },
                    { "contempt", emotionAttributes.contempt },
                    { "disgust", emotionAttributes.disgust },
                    { "fear", emotionAttributes.fear },
                    {"happiness", emotionAttributes.happiness },
                    {"sadness", emotionAttributes.sadness },
                    {"suprise", emotionAttributes.surprise }
                };

                emotionName = emotions.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //Message
                message += string.Format("{0} looks {1} years old and feels {2}", face.faceAttributes.gender == 0 ? "He" : "She", face.faceAttributes.age,
                emotionName);
            }
        }
        else
            message = "I could't detect anyone.";

        // Try and get a TTS Manager
        // TextToSpeech tts = ttsManager;

        if (ttsManager != null) {
            //Play voice message
            ttsManager.StartSpeaking(message);
        }
    }
}
