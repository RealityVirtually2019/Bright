// Starts the default camera and assigns the texture to the current renderer
using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour {
  WebCamTexture webcamTexture;

    void Start() {
        Debug.Log("Initiating Zoom");

        webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    // zoom in
    void OnEnable() {
        Debug.Log("Enabling Zoom");

        if (webcamTexture && !webcamTexture.isPlaying)
        {
            webcamTexture.Play();
        }
    }

    // stop
    void OnDisable() {
        Debug.Log("Disabling Zoom");

        webcamTexture.Stop();
    }

    public void Toggle() {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public byte[] getImageData() {
        if (!webcamTexture.isPlaying) {
            Debug.LogError("Trying to get Image while Webcamtexture is not running!");
        }

        Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height);
        snap.SetPixels(webcamTexture.GetPixels());
        snap.Apply();
        return ImageConversion.EncodeToPNG(snap);
  }
}
