using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaleUI_actions : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { 
            this.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
