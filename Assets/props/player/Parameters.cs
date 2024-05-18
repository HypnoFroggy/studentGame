using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    // Start is called before the first frame update
    public float hunger;
    public float happiness;
    public float time;
    public float thirst;
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
        hunger -= 0.01f * Time.deltaTime;
        thirst -= 0.01f * Time.deltaTime;
        happiness -= 0.01f * Time.deltaTime;
        time += 1;
    }
}
