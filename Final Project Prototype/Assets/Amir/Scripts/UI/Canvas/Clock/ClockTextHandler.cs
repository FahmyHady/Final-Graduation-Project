using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTextHandler : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] ClockManager clockManager;
    void Start()
    {
        UpdateTime();
    }
    public void UpdateTime() {
        text.text = clockManager.GetTime();
        if (text.text == "00:30")
            text.color = Color.red;
    }
    // Update is called once per frame
  
}
