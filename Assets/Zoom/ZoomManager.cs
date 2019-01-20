using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    public GameObject zoomPlane;
    public GameObject zoomObject;

    private float maxZoomFactor = 2.0f;
    private float minZoomFactor = 0.5f;
    private float zoomFactor = 1.0f;

    public void tryZoomIn() {
        zoomPlane.SetActive(true);

        if (zoomFactor <= maxZoomFactor) {
            zoomFactor += 0.25F;
            ApplyZoom();
        } else {
            //can't zoom in more
        }
    }

    public void zoomOut() {
        if (zoomFactor >= minZoomFactor) {
            zoomFactor -= 0.25F;
            ApplyZoom();
        } else {
            // stop camera when smaller than the start size
            zoomPlane.SetActive(false);
        }
    }

    private void ApplyZoom() {
        zoomObject.transform.localScale = Vector3.one * zoomFactor;
    }
}
