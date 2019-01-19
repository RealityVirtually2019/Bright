// Modified from https://github.com/wontonst/twilio-sms-unity

using UnityEngine;
using System.Collections;

public class TwilioSMS : MonoBehaviour
{
  string url = "api.twilio.com/2010-04-01/Accounts/";
  string service = "/Messages.json";
  public string from;
  public string to;
  public string account_sid;
  public string auth;

  public void SendSMS(string body)
  {
    WWWForm form = new WWWForm();
    form.AddField("To", to);
    form.AddField("From", from);
    //string bodyWithoutSpace = body.Replace (" ", "%20");//Twilio doesn't need this conversion
    form.AddField("Body", body);
    string completeurl = "https://" + account_sid + ":" + auth + "@" + url + account_sid + service;
    Debug.Log(completeurl);
    WWW www = new WWW(completeurl, form);
    StartCoroutine(WaitForRequest(www));
  }

  IEnumerator WaitForRequest(WWW www)
  {
    yield return www;

    // check for errors
    if (www.error == null)
    {
      Debug.Log("WWW Ok! SMS sent through Web API: " + www.text);
    }
    else
    {
      Debug.Log("WWW Error: " + www.error);
    }
  }
}