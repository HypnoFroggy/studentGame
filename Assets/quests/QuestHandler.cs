using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.Search;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public Parameters PlayerParameters;

    public QuestManager manager;
    public int id;
    public string title;
    public string tale;
    public Dictionary<string, int> modifiers;
    public List<string> nextQuestsAdress;

    private const int maxTitleLength = 45; //if 1280*720 , 36 pt
    private const int maxTaleLength = 500; //if 1280*720 , 36 pt

    public void ShowTale()
    {
        Time.timeScale = 0.2f;
        manager.questUI.SetActive(true);
        foreach (TMP_Text text in manager.questUI.GetComponentsInChildren<TMP_Text>())
        {
            if(text.gameObject.name == "title")
            {
                if (title.Length > maxTitleLength)
                {
                    string[] words = title.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string res = "";
                    foreach (string word in words)
                    {
                        if (res.Length + word.Length < maxTitleLength)
                        {
                            res += word+' ';
                        }
                        else
                        {
                            break;
                        }
                    }
                    text.text = res.Trim();
                }
                else
                {
                    text.text = title;
                }

            }
            if(text.gameObject.name == "tale")
            {
                if(tale.Length > maxTaleLength)
                {
                    string[] words = tale.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string res = "";
                    foreach (string word in words)
                    {
                        if (res.Length + word.Length < maxTaleLength)
                        {
                            res += word + ' ';
                        }
                        else
                        {
                            break;
                        }
                    }
                    text.text = res.Trim();
                }
                else
                {
                    text.text = tale;
                }
            }
        }

    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowTale();
            foreach (var modName in modifiers.Keys)
            {
                switch (modName)
                {
                    case "thirst":
                        PlayerParameters.thirst += modifiers[modName];
                        break;
                    case "hunger":
                        PlayerParameters.hunger += modifiers[modName];
                        break;
                    case "happiness":
                        PlayerParameters.happiness += modifiers[modName];
                        break;
                    case "time":
                        PlayerParameters.time += modifiers[modName];
                        break;
                    case "lastSleep":
                        PlayerParameters.lastSleep = (int)(PlayerParameters.time % Parameters.ms_in_day) + 1 - modifiers[modName];
                        break;
                    default:
                        break;
                }
            }
            manager.activeQuests.Remove(id);
            foreach (var addr in nextQuestsAdress)
            {
                manager.CreateNewQuest(addr);
            }
            Destroy(Array.Find(this.gameObject.GetComponentsInParent<Component>(), go => go.gameObject.name.Contains("QuestObject")).gameObject);
        }
    }
}
