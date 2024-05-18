using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public GameObject questmark;
    public Parameters PlayerParameters;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerParameters.thirst += 20;
        Destroy(questmark);
    }
}
