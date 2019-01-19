// Modified from https://github.com/wontonst/twilio-sms-unity

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Twilio : MonoBehaviour
{
  string apiUrl = "api.twilio.com/2010-04-01/Accounts/";
  string smsService = "/Messages.json";
  string callService = "/Calls.json";
  public string from;
  public string to;

  string authenticate()
  {
    // Retrieve auth information from JSON
    string username = Globals.instance.credentials.twilio_account_sid;
    string password = Globals.instance.credentials.twilio_auth;

    string auth = username + ":" + password;
    auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
    auth = "Basic " + auth;
    return auth;
  }

  // The url has to point to an xml file containing instructions on what twilio should say
  // If the file is hosted on github, changes will only take effect AFTER pushing changes there.
  public void SendSMS(string url)
  {
    StartCoroutine(DoSendSMS(url));
  }
  IEnumerator DoSendSMS(string body)
  {
    string authorization = authenticate();

    WWWForm form = new WWWForm();
    form.AddField("To", to);
    form.AddField("From", from);
    form.AddField("Body", body);

    string completeurl = "https://" + apiUrl + Globals.instance.credentials.twilio_account_sid + smsService;
    Debug.Log(completeurl);

    using (UnityWebRequest www = UnityWebRequest.Post(completeurl, form))
    {
      www.SetRequestHeader("AUTHORIZATION", authorization);

      yield return www.SendWebRequest();

      if (www.isNetworkError || www.isHttpError)
      {
        Debug.Log(www.error);
      }
      else
      {
        // Show results as text
        Debug.Log("Response" + www.downloadHandler.text);
      }
    }
  }

  // The url has to point to an xml file containing instructions on what twilio should say
  // If the file is hosted on github, changes will only take effect AFTER pushing changes there.
  public void MakeCall(string url)
  {
    StartCoroutine(DoMakeCall(url));
  }
  IEnumerator DoMakeCall(string speechUrl)
  {
    string authorization = authenticate();

    WWWForm form = new WWWForm();
    form.AddField("To", to);
    form.AddField("From", from);
    form.AddField("Url", speechUrl);

    string completeurl = "https://" + apiUrl + Globals.instance.credentials.twilio_account_sid + callService;
    Debug.Log(completeurl);

    using (UnityWebRequest www = UnityWebRequest.Post(completeurl, form))
    {
      www.SetRequestHeader("AUTHORIZATION", authorization);

      yield return www.SendWebRequest();

      if (www.isNetworkError || www.isHttpError)
      {
        Debug.Log(www.error);
      }
      else
      {
        // Show results as text
        Debug.Log("Response" + www.downloadHandler.text);
      }
    }
  }
}