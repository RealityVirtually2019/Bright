using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;

public class PhotoCaptureHandler : MonoBehaviour
{
    WebCamTexture webcamTexture;
    PhotoCapture photoCapture = null;

    //void Start() {
    //    Debug.Log("Initiating PhotoCapture");

    //    webcamTexture = new WebCamTexture();
    //    Renderer renderer = GetComponent<Renderer>();
    //    renderer.material.mainTexture = webcamTexture;
    //    webcamTexture.Play();
    //}

    //void OnEnable() {
    //    Debug.Log("Enabling PhotoCapture");
    //    StartPhotoCapture();
    //}

    // Don't call this directly; see functions below, which set the callback to call directly!
    private void StartPhotoCapture()
    {
        Debug.Log("Enabling PhotoCapture");
        //Create capture async
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        Debug.Log("OnPhotoCaptureCreated");
        photoCapture = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.JPEG;

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCapture.Dispose();
        photoCapture = null;
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("OnPhotoModeStarted");
        /* Load to Memory */
        if (result.success)
        {
            try
            {
                photoCapture.TakePhotoAsync(OnCapturedPhotoToMemory);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("System.ArgumentException:\n" + e.Message);
            }
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!");
            photoCapture.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        else
        {
            Debug.Log("Failed to save Photo to disk");
        }
    }

  void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            List<byte> imageBufferList = new List<byte>();

            Debug.Log("OnCapturedPhotoToMemory Copy Started");

            photoCaptureFrame.CopyRawImageDataIntoBuffer(imageBufferList);

            Debug.Log("OnCapturedPhotoToMemory " + imageBufferList.Count);

            //Execute Face Coroutine
            ExecuteCallback(imageBufferList);
        }
        else
        {
            Debug.Log("Failed to save Photo to memory");
        }

        photoCapture.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    private delegate void PhotoCallbackFunction(byte[] imageData);
    PhotoCallbackFunction callbackFunction;
    private void ExecuteCallback(List<byte> imageBufferList)
    {
        Debug.Log("Started PostToFaceAPI processing");
        callbackFunction(imageBufferList.ToArray());
        Debug.Log("Ended PostToFaceAPI processing coroutine");
    }

  // Public function to be called in order to trigger FaceRecognition
  public void DoFaceRecognition()
    {
        Globals.instance.textToSpeech.StartSpeaking("Searching for Faces.");
        callbackFunction = ExecuteFaceTracking;
        StartPhotoCapture();
    }

  // Internal Callback Function
  private void ExecuteFaceTracking(byte[] imageData) {
        CognitiveServices cognitiveServices = new CognitiveServices();
        StartCoroutine(cognitiveServices.PostToFace(imageData));
    }

    // Public function to be called in order to trigger OCR
  public void DoOCR()
  {
    Globals.instance.textToSpeech.StartSpeaking("Searching for Text.");
    callbackFunction = ExecuteOCR;
    StartPhotoCapture();
  }

    // Internal Callback Function
  private void ExecuteOCR(byte[] imageData)
  {
    CognitiveServices cognitiveServices = new CognitiveServices();
    StartCoroutine(cognitiveServices.PostToOCR(imageData));
  }

}
