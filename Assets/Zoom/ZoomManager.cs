using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    public GameObject zoomPlane;
    public GameObject zoomObject;

    private Vector3 maxScale = new Vector3(2.0F, 2.0F, 2.0F);
    private Vector3 minScale = new Vector3(0.0F, 0.0F, 0.0F);

    public void tryZoomIn() {
        zoomPlane.SetActive(true);

        if (zoomObject.transform.localScale != maxScale) {
            zoomObject.transform.localScale += new Vector3(0.25F, 0.25F, 0.25F);
        } else {
            //can't zoom in more
        }
    }

    public void zoomOut() {
        if (zoomObject.transform.localScale != minScale) {
            zoomObject.transform.localScale -= new Vector3(0.25F, 0.25F, 0.25F);
        } else {
            // stop camera when smaller than the start size
            zoomPlane.SetActive(false);
        }
    }
}
