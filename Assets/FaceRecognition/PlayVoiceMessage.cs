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

    public void PlayTextToSpeechMessage(FaceObject face)
    {
        string message = string.Empty;
        string emotionName = string.Empty;

        if (face.faces.Count > 0)
        {
            EmotionAttributes emotionAttributes = face.faces[0].emotionAttributes;

            Dictionary<string, float> emotions = new Dictionary<string, float>
            {
                { "anger", emotionAttributes.anger },
                { "contempt", emotionAttributes.contempt },
                { "disgust", emotionAttributes.disgust },
                { "fear", emotionAttributes.fear },
                {"happiness", emotionAttributes.happiness },
                {"sadness", emotionAttributes.sadness },
                {"suprise", emotionAttributes.surprise }
            };

            emotionName = emotions.Keys.Max();

            //Message
            message = string.Format("{0} looks {1} years old and feels {2}", face.faces[0].faceAttributes.gender == 0 ? "He" : "She", face.faces[0].faceAttributes.age,
            emotionName);
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