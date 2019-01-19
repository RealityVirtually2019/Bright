// Based upon https://www.rageagainstthepixel.com/mrtk-dictation-input/

using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class SpeechToText : MonoBehaviour, IDictationHandler
{
  [SerializeField]
  [Range(0.1f, 5f)]
  [Tooltip("The time length in seconds before dictation recognizer session ends due to lack of audio input in case there was no audio heard in the current session.")]
  private float initialSilenceTimeout = 5f;
  [SerializeField]
  [Range(5f, 60f)]
  [Tooltip("The time length in seconds before dictation recognizer session ends due to lack of audio input.")]
  private float autoSilenceTimeout = 20f;
  [SerializeField]
  [Range(1, 60)]
  [Tooltip("Length in seconds for the manager to listen.")]
  public int recordingTime = 60;
  private string lastOutput;
  private string speechToTextOutput = string.Empty;
  public string SpeechToTextOutput { get { return speechToTextOutput; } }
  private bool isRecording;

  public GameObject listener;
  public Text uiText;

  public void ToggleRecording()
  {
    if (isRecording)
    {
      StartListening();
    }
    else
    {
      StopListening();
    }
  }

  public void StartListening () {
    isRecording = true;
    StartCoroutine(DictationInputManager.StartRecording(listener, initialSilenceTimeout, autoSilenceTimeout, recordingTime));
  }


  public void StopListening()
  {
    isRecording = false;
    StartCoroutine(DictationInputManager.StopRecording());
  }

  void IDictationHandler.OnDictationHypothesis(DictationEventData eventData)
  {
    speechToTextOutput = eventData.DictationResult;
  }
  void IDictationHandler.OnDictationResult(DictationEventData eventData)
  {
    speechToTextOutput = eventData.DictationResult;
  }
  void IDictationHandler.OnDictationComplete(DictationEventData eventData)
  {
    speechToTextOutput = eventData.DictationResult;
  }
  void IDictationHandler.OnDictationError(DictationEventData eventData)
  {
    isRecording = false;
    speechToTextOutput = eventData.DictationResult;
    Debug.LogError(eventData.DictationResult);
    StartCoroutine(DictationInputManager.StopRecording());
  }

  private void Update()
  {
    if (!string.IsNullOrEmpty(speechToTextOutput) && !lastOutput.Equals(speechToTextOutput))
    {
      Debug.Log(speechToTextOutput);
      lastOutput = speechToTextOutput;

      uiText.text = speechToTextOutput;
    }
  }
}