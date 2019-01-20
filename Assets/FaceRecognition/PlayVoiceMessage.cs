using Newtonsoft.Json;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayVoiceMessage : MonoBehaviour {

    public static PlayVoiceMessage Instance { get; private set; }

    public TextToSpeech ttsManager;

    /// <summary>
    /// The analysis result text
    /// </summary>
    private TextMesh labelText;

    /// <summary>
    /// Base endpoint of Face Recognition Service
    /// </summary>
    const string baseEndpoint = "https://westus.api.cognitive.microsoft.com/face/v1.0/";

    /// <summary>
    /// Id (name) of the created person group
    /// </summary>
    private const string personGroupId = "0";

    /// <summary>
    /// Collection of faces that needs to be identified
    /// </summary>
    public class FacesToIdentify_RootObject
    {
        public string personGroupId { get; set; }
        public List<string> faceIds { get; set; }
        public int maxNumOfCandidatesReturned { get; set; }
        public double confidenceThreshold { get; set; }
    }

    /// <summary>
    /// Collection of Candidates for the face
    /// </summary>
    public class Candidate_RootObject
    {
        public string faceId { get; set; }
        public List<Candidate> candidates { get; set; }
    }

    public class Candidate
    {
        public string personId { get; set; }
        public double confidence { get; set; }
    }

    /// <summary>
    /// Name and Id of the identified Person
    /// </summary>
    public class IdentifiedPerson_RootObject
    {
        public string personId { get; set; }
        public string name { get; set; }
    }

    void Awake()
    {
        Instance = this;

        // Create the text label in the scene
        CreateLabel();
    }

    /// <summary>
    /// Spawns cursor for the Main Camera
    /// </summary>
    private void CreateLabel()
    {
        // Create a sphere as new cursor
        GameObject newLabel = new GameObject();

        // Attach the label to the Main Camera
        newLabel.transform.parent = gameObject.transform;

        // Resize and position the new cursor
        newLabel.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        newLabel.transform.position = new Vector3(0f, 3f, 60f);

        // Creating the text of the Label
        labelText = newLabel.AddComponent<TextMesh>();
        labelText.anchor = TextAnchor.MiddleCenter;
        labelText.alignment = TextAlignment.Center;
        labelText.tabSize = 4;
        labelText.fontSize = 50;
        labelText.text = ".";
    }

    public void PlayTextToSpeechMessage(FaceObject faceObj)
    {
        string message = string.Empty;
        string emotionName = string.Empty;

        if (faceObj.faces.Count > 0)
        {
            IdentifyFaces(faceObj.faces);

            // foreach (Face face in faceObj.faces) {
            //     EmotionAttributes emotionAttributes = face.emotionAttributes;
            //
            //     Dictionary<string, float> emotions = new Dictionary<string, float>
            // {
            //     { "neutral", emotionAttributes.neutral },
            //     { "anger", emotionAttributes.anger },
            //     { "contempt", emotionAttributes.contempt },
            //     { "disgust", emotionAttributes.disgust },
            //     { "fear", emotionAttributes.fear },
            //     {"happiness", emotionAttributes.happiness },
            //     {"sadness", emotionAttributes.sadness },
            //     {"suprise", emotionAttributes.surprise }
            // };

            //     emotionName = emotions.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            //
            //     //Message
            //     message += string.Format("{0} looks {1} years old and feels {2}", face.faceAttributes.gender == 0 ? "He" : "She", face.faceAttributes.age,
            //     emotionName);
            // }
        }
        else
            message = "I couldn't detect anyone.";

        // Try and get a TTS Manager
        // TextToSpeech tts = ttsManager;

        if (ttsManager != null) {
            //Play voice message
            ttsManager.StartSpeaking(message);
        }
    }

    /// <summary>
    /// Identify the faces found in the image within the person group
    /// </summary>
    internal IEnumerator IdentifyFaces(List<Face> listOfFacesToIdentify)
    {
        // Create the object hosting the faces to identify
        FacesToIdentify_RootObject facesToIdentify = new FacesToIdentify_RootObject();
        facesToIdentify.faceIds = new List<string>();
        facesToIdentify.personGroupId = personGroupId;
        foreach (Face face in listOfFacesToIdentify)
        {
            facesToIdentify.faceIds.Add(face.faceId);
        }
        facesToIdentify.maxNumOfCandidatesReturned = 1;
        facesToIdentify.confidenceThreshold = 0.5;

        // Serialise to Json format
        string facesToIdentifyJson = JsonConvert.SerializeObject(facesToIdentify);
        // Change the object into a bytes array
        byte[] facesData = Encoding.UTF8.GetBytes(facesToIdentifyJson);

        WWWForm webForm = new WWWForm();
        string detectFacesEndpoint = $"{baseEndpoint}identify";

        using (UnityWebRequest www = UnityWebRequest.Post(detectFacesEndpoint, webForm))
        {
            www.SetRequestHeader("Ocp-Apim-Subscription-Key", Constants.MCS_FACEKEY);
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(facesData);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();
            string jsonResponse = www.downloadHandler.text;
            Debug.Log($"Get Person - jsonResponse: {jsonResponse}");
            Candidate_RootObject [] candidate_RootObject = JsonConvert.DeserializeObject<Candidate_RootObject[]>(jsonResponse);

            // For each face to identify that ahs been submitted, display its candidate
            foreach (Candidate_RootObject candidateRO in candidate_RootObject)
            {
                StartCoroutine(GetPerson(candidateRO.candidates[0].personId));

                // Delay the next "GetPerson" call, so all faces candidate are displayed properly
                yield return new WaitForSeconds(3);
            }
        }
    }

    /// <summary>
    /// Provided a personId, retrieve the person name associated with it
    /// </summary>
    internal IEnumerator GetPerson(string personId)
    {
        string getGroupEndpoint = $"{baseEndpoint}persongroups/{personGroupId}/persons/{personId}?";
        WWWForm webForm = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get(getGroupEndpoint))
        {
            www.SetRequestHeader("Ocp-Apim-Subscription-Key", Constants.MCS_FACEKEY);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            string jsonResponse = www.downloadHandler.text;

            Debug.Log($"Get Person - jsonResponse: {jsonResponse}");
            IdentifiedPerson_RootObject identifiedPerson_RootObject = JsonConvert.DeserializeObject<IdentifiedPerson_RootObject>(jsonResponse);

            // Display the name of the person in the UI
            labelText.text = identifiedPerson_RootObject.name;
            ttsManager.StartSpeaking("This is {identifiedPerson_RootObject.name}.");
        }
    }
}
