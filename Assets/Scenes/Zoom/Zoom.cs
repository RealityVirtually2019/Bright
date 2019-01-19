// Starts the default camera and assigns the texture to the current renderer
using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour {
  WebCamTexture webcamTexture;

    void Start() {
    }

    void OnEnable() {
        Debug.Log("Enabling Zoom");

        webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void OnDisable() {
        Debug.Log("Disabling Zoom");
        
        webcamTexture.Stop();
    }
}