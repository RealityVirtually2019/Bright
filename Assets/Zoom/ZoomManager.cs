﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    public GameObject zoomPlane;
    public GameObject zoomObject;

    private float maxZoomFactor = 2.0f;
    private float minZoomFactor = 0.5f;
    private float zoomFactor = .5f;
    public float zoomStep = .5f;

    public void tryZoomIn() {
        zoomPlane.SetActive(true);

        if (zoomFactor <= maxZoomFactor) {
            Globals.instance.textToSpeech.StartSpeaking("Zooming in");
            zoomFactor += zoomStep;
            ApplyZoom();
        } else {
            //can't zoom in more
            Globals.instance.textToSpeech.StartSpeaking("Can't zoom in any more");
        }
    }

    public void zoomOut() {
        if (zoomFactor >= minZoomFactor) {
            Globals.instance.textToSpeech.StartSpeaking("Zooming out");
            zoomFactor -= zoomStep;
            ApplyZoom();
        } else {
            // stop camera when smaller than the start size
            StopZoom();
        }
    }

    public void StopZoom()
    {
        Globals.instance.textToSpeech.StartSpeaking("Stopping Zoom");
        zoomPlane.SetActive(false);
    }

    private void ApplyZoom() {
        Debug.Log("Applzing Zoom");
        zoomObject.transform.localScale = Vector3.one * zoomFactor;
    }
}
