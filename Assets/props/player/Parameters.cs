using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    // Start is called before the first frame update
    public const int ms_in_sec = 1; // minimum unit - sec 1000;
    public const int ms_in_min = ms_in_sec * 60;
    public const int ms_in_hour = ms_in_min * 60;
    public const int ms_in_day = ms_in_hour * 24;


    public float hunger;
    public float happiness;
    public float time;
    public float thirst;
    public float coef = 0.1f;
    public long lastSleep;
    void Start()
    {
        hunger = 100f;
        happiness = 100f;
        time = 0f;
        thirst = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= coef * Time.deltaTime;
        thirst -= coef * Time.deltaTime;
        happiness -= coef * Time.deltaTime;
        time += 1;
        if (lastSleep < (int)(time % ms_in_day))
        {
            happiness -= coef * ((int)(time / ms_in_day) - lastSleep) * Time.deltaTime;
        }
    }
}
