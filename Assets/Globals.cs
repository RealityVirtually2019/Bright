using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

// Singleton to hold global variables
public class Globals : MonoBehaviour
{
    public static Globals instance;

    [System.NonSerialized]
    public Credentials credentials;
    public TextAsset credentialsFile;
    public TextToSpeech textToSpeech;

    // Start is called before the first frame update
    void OnEnable() {
        instance = this;

        if (!credentialsFile) {
            Debug.LogError("Error: Missing Credentials.json!");
        } else {
            Debug.Log("Loading Credentials: " + credentialsFile.text);

            credentials = Credentials.CreateFromJSON(credentialsFile.text);
        }
    }

}
