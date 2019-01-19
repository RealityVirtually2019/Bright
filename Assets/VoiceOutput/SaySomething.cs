using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaySomething : MonoBehaviour
{
    public TextToSpeech textToSpeech;
  
    void Start()
    {
        textToSpeech.StartSpeaking("Welcome to Bright.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
