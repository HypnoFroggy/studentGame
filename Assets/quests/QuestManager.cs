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
    /*public static QuestInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestInfo>(jsonString);
    }*/

    /*public static QuestInfo MyCreateFromJSON(string jsonString)
    {
        QuestInfo questData = new QuestInfo();
        string field_name="";
        string field_value="";
        List<string> addrs = new List<string>();
        List<float> pos = new List<float>();
        bool is_name = true;
        bool is_write = false;
        bool is_array = false;
        bool is_dict = false;
        for(int char_index = 1; char_index<jsonString.Length-1; char_index++)
        {
            if (jsonString[char_index] == '\"')
            {
                is_write = !is_write;
                continue;
            }
            if (jsonString[char_index] == ':')
            {
                is_name = false;
                if((field_name!="tale")&&(field_name!="title")&&(field_name!= "nextQuestsAdress"))
                {
                    is_write=true;
                }
                continue;
            }
            if (jsonString[char_index] == ',')
            {
                if (is_dict)
                {
                    if (questData.modifiers.ContainsKey(field_name))
                    {
                        questData.modifiers[field_name] = int.Parse(field_value);
                    }
                    else
                    {
                        questData.modifiers.Add(field_name, int.Parse(field_value));
                    }
                    field_name = "";
                    field_value = "";
                    is_name = true;
                    is_write = false;
                    continue;
                }
                if (is_array)
                {
                    if(field_name == "spawnPos")
                    {
                        Debug.Log($"{field_name}_{is_name}_{is_write}_{field_value}");
                        pos.Add(float.Parse(field_value));
                    }
                    if(field_name== "nextQuestsAdress")
                    {
                        addrs.Add(field_value);
                    }
                    field_value = "";
                    is_name = false;
                    continue;
                }
                if (field_name == "id")
                {
                    //Debug.Log($"{field_name}_{is_name}_{is_write}_{field_value}");
                    questData.id = int.Parse(field_value);
                    field_name = "";
                    field_value = "";
                    is_write = false;
                    is_name = true;
                    continue;
                }
                if (field_name == "scene")
                {
                    Debug.Log($"{field_value}");
                    questData.scene = int.Parse(field_value);
                    field_name = "";
                    field_value = "";
                    is_write = false;
                    is_name = true;
                    continue;
                }
                if (field_name == "title")
                {
                    questData.title = field_value;
                    field_name = "";
                    field_value = "";
                    is_write = false;
                    is_name = true;
                    continue;
                }
                if (field_name == "tale")
                {
                    questData.tale = field_value;
                    field_name = "";
                    field_value = "";
                    is_write = false;
                    is_name = true;
                    continue;
                }
            }
            if((jsonString[char_index] == '{')&&(field_name== "modifiers"))
            {
                is_dict = true;
                is_name = true;
                continue;
            }
            if (jsonString[char_index] == '}')
            {
                is_dict= false;
                continue;
            }
            if ((jsonString[char_index] == '[') && ((field_name == "spawnPos")||(field_name== "nextQuestsAdress")))
            {
                Debug.Log($"{field_name}");
                if (field_name == "spawnPos")
                {
                    is_write = true;
                }
                is_array = true;
                continue;
            }
            if (jsonString[char_index] == ']')
            {
                is_array = false;
                continue;
            }
            if ((jsonString[char_index] == ' ') && (!is_write))
            {
                continue;
            }
            if (is_write)
            {
                if (is_name)
                {
                    field_name += jsonString[char_index];
                }
                else
                {
                    field_value += jsonString[char_index];
                }
            }
        }
        questData.nextQuestsAdress = addrs;
        if (pos.Count < 3)
        {
            while (pos.Count < 3)
            {
                pos.Add(0);
            }
            Debug.Log("quest pos err");
        }
        questData.spawnPos=new Vector3(pos[0], pos[1], pos[2]);
        return questData;

        //return JsonUtility.FromJson<QuestInfo>(jsonString);
    }
    */
    // Start is called before the first frame update
    
    public static QuestInfo MyCreateFromSystemJSON(string jsonString)
    {
        QuestInfo questData = JsonConvert.DeserializeObject<QuestInfo>(jsonString);
        return questData;
    }
    public void CreateNewQuest(string addr)
    {
        string jsonString = File.ReadAllText(addr);
        QuestInfo questData = MyCreateFromSystemJSON(jsonString);
        Debug.Log($"questData: {questData.id} {questData.title}");
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
