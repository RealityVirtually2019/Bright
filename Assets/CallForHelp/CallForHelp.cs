using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForHelp : MonoBehaviour
{
    public Twilio twilio;
    public string onlineUrlToSpeechXML = "https://raw.githubusercontent.com/RealityVirtually2019/Bright/master/Assets/CallForHelp/twilioSpeech.xml";
    // Start is called before the first frame update
    void Call()
    {
        twilio.MakeCall(onlineUrlToSpeechXML);
    }
}
