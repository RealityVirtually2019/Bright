using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCycler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Text text;
    public List<string> strings;
    public float displayTimePerItem = 4f;

    float t = 0;
    void Update()
    {
        t += Time.deltaTime;
        if (t >= displayTimePerItem)
        {
            t = 0;

            if (strings.Count > 0) {
                text.text = strings[0];
                strings.RemoveAt(0);
            } else {
                text.text = "";
            }
        }
    }
}
