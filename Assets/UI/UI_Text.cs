using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Text : MonoBehaviour
{
    public Text thirstbox;
    public Text hungerbox;
    public Text happinessbox;
    public Text timebox;
    public Parameters PlayerParameters;

    private const int ms_in_sec = 1000;
    private const int ms_in_min = ms_in_sec*60;
    private const int ms_in_hour = ms_in_min*60;
    private const int ms_in_day = ms_in_hour*24;
    void Update()
    {
        thirstbox.text = (PlayerParameters.thirst).ToString("N2");
        hungerbox.text = (PlayerParameters.hunger).ToString("N2");
        float time = PlayerParameters.time;
        timebox.text = string.Format("Δενό {0} {1:D2}:{2:D2}:{3:D2}", (int)(time/ms_in_day)+1, (int)((time % ms_in_day)/ms_in_hour), (int)((time % ms_in_hour) / ms_in_min), (int)((time % ms_in_min)/ms_in_sec));
        happinessbox.text = (PlayerParameters.happiness).ToString("N2");
    }
}
