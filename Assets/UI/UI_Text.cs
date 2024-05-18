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

    void Update()
    {
        thirstbox.text = PlayerParameters.thirst.ToString();
        hungerbox.text = PlayerParameters.hunger.ToString();
        timebox.text = PlayerParameters.time.ToString();
        happinessbox.text = PlayerParameters.happiness.ToString();
    }
}
