using UnityEngine;

[System.Serializable]
public class Credentials
{
  public string twilio_account_sid;
  public string twilio_auth;
  public static Credentials CreateFromJSON(string jsonString)
  {
    return JsonUtility.FromJson<Credentials>(jsonString);
  }
}