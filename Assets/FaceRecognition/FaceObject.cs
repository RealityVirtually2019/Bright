using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour {
    public List<Face> faces { get; set; }
}

public class Face {
    public string faceId { get; set; }
    public FaceRectangle faceRectangle { get; set; }
    public FaceAttributes faceAttributes { get; set; }
    public EmotionAttributes emotionAttributes { get; set; }
}

public class FaceRectangle {
    public int top { get; set; }
    public int left { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class FaceAttributes {
    public int gender { get; set; } // male = 0 , female = 1
    public float age { get; set; }
    public FacialHair facialHair { get; set; }
}

public class EmotionAttributes {
    public float anger { get; set; }
    public float contempt { get; set; }
    public float disgust { get; set; }
    public float fear { get; set; }
    public float happiness { get; set; }
    public float neutral { get; set; }
    public float sadness { get; set; }
    public float surprise { get; set; }
}

public class HeadPose {
    public float pitch { get; set; }
    public float roll { get; set; }
    public float yaw { get; set; }
}

public class FacialHair {
    public bool hasMoustache { get; set; }
    public bool hasBeard { get; set; }
    public bool hasSideburns { get; set; }
}
