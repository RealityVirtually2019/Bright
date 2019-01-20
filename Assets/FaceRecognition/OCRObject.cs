// Based upon: https://github.com/gntakakis/Hololens-MSCognitiveServicesOCR-Unity/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OCRObject : MonoBehaviour {

    public string language { get; set; }
    public string textAngle { get; set; }
    public string orientation { get; set; }
    public string text { get; set; }
}