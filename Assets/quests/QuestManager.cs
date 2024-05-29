using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class QuestManager : MonoBehaviour
{
    public GameObject questPrefab; 
    public GameObject questUI;
    public Parameters parameters;

    public string startQuestAdress = "./Assets/questlines/start.json";
    public Dictionary<int, GameObject> activeQuests = new Dictionary<int, GameObject>();
    public Dictionary<int, HashSet<string>> inactiveQuestsForOtherscene = new Dictionary<int, HashSet<string>>();
    [Serializable]
    public class QuestInfo
    {
        public string title;
        public int id;
        public string tale;
        public Dictionary<string,int> modifiers = new Dictionary<string,int>();
        public int scene;
        public float[] spawnPos = new float[3];
        public List<string> nextQuestsAdress = new List<string>();
    }
    public static QuestInfo MyCreateFromSystemJSON(string jsonString)
    {
        QuestInfo questData = JsonConvert.DeserializeObject<QuestInfo>(jsonString);
        return questData;
    }
    public void CreateNewQuest(string addr)
    {
        string jsonString = File.ReadAllText(addr);
        QuestInfo questData = MyCreateFromSystemJSON(jsonString);
        if(questData.scene!= SceneManager.GetActiveScene().buildIndex)
        {
            if (!inactiveQuestsForOtherscene.ContainsKey(questData.scene))
            {
                inactiveQuestsForOtherscene.Add(questData.scene, new HashSet<string>());
            }
            inactiveQuestsForOtherscene[questData.scene].Add(addr);
            return;
        }

        if (!activeQuests.ContainsKey(questData.id))
        {
            GameObject newQuest = Instantiate(questPrefab, new Vector3(questData.spawnPos[0], questData.spawnPos[1], questData.spawnPos[2]), new Quaternion(0, 0, 0, 0));
            //newQuest.transform.position = questData.spawnPos;
            QuestHandler newQuestHandler = newQuest.gameObject.GetComponentInChildren<QuestHandler>();

            newQuestHandler.title = questData.title;
            newQuestHandler.id = questData.id;
            newQuestHandler.tale = questData.tale;
            newQuestHandler.modifiers = questData.modifiers;
            newQuestHandler.nextQuestsAdress = questData.nextQuestsAdress;

            newQuestHandler.PlayerParameters = parameters;
            newQuestHandler.manager = this;

            activeQuests.Add(questData.id, newQuest);
        }
    }

    void Start()
    {
        string jsonString = File.ReadAllText(startQuestAdress);
        QuestInfo startQuestData = MyCreateFromSystemJSON(jsonString);
        foreach (var addr in startQuestData.nextQuestsAdress)
        {
            CreateNewQuest(addr);
        }
        if (inactiveQuestsForOtherscene.ContainsKey(SceneManager.GetActiveScene().buildIndex))
        {
            foreach (var addr in inactiveQuestsForOtherscene[SceneManager.GetActiveScene().buildIndex])
            {
                CreateNewQuest(addr);
            }
        }
    }
}
