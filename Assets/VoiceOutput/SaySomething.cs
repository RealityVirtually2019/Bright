using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaySomething : MonoBehaviour
{
    public TextToSpeech textToSpeech;
  
    void Start()
    {
        textToSpeech.StartSpeaking("Hello RealitzVirtually 2019 yeah");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
