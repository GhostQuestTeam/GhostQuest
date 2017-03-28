using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class SaveGamestateController : MonoBehaviour {

    void OnApplicationQuit()
    {
        QuestManager.SaveQuests();
    }
}
