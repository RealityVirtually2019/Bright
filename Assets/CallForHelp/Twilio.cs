// Modified from https://github.com/wontonst/twilio-sms-unity

using UnityEngine;
using System.Collections;

public class Twilio : MonoBehaviour
{
  string apiUrl = "api.twilio.com/2010-04-01/Accounts/";
  string smsService = "/Messages.json";
  string callService = "/Calls.json";
  public string from;
  public string to;

  public void SendSMS(string body)
  {
    // Retrieve auth information from JSON
    string account_sid = Globals.instance.credentials.twilio_account_sid;
    string auth = Globals.instance.credentials.twilio_auth;

    WWWForm form = new WWWForm();
    form.AddField("To", to);
    form.AddField("From", from);
    form.AddField("Body", body);
    string completeurl = "https://" + account_sid + ":" + auth + "@" + apiUrl + account_sid + smsService;
    Debug.Log(completeurl);
    WWW www = new WWW(completeurl, form);
    StartCoroutine(WaitForRequest(www));
  }

  // The url has to point to an xml file containing instructions on what twilio should say
  // If the file is hosted on github, changes will only take effect AFTER pushing changes there.
  public void MakeCall(string url)
  {
    // Retrieve auth information from JSON
    string account_sid = Globals.instance.credentials.twilio_account_sid;
    string auth = Globals.instance.credentials.twilio_auth;

    WWWForm form = new WWWForm();
    form.AddField("To", to);
    form.AddField("From", from);
    form.AddField("Url", url);
    string completeurl = "https://" + account_sid + ":" + auth + "@" + apiUrl + account_sid + callService;
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
      Debug.Log("WWW Success, Response: " + www.text);
    }
    else
    {
      Debug.Log("WWW Error: " + www.error + " Response: " + www.text);
    }
  }
}