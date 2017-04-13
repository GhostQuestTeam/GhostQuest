using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using QuestSystem;
using UnityEngine;

public class ExampleDialogueObserver : MonoBehaviour
{


    public GameObject DialoquePanel;
	// Use this for initialization
	void Start ()
	{
	    DialoquePanel.GetComponent<DialogueView>().OnAnswerChoose += OnAnswerChoose;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnAnswerChoose(object sender, DialogueEventArgs dea)
    {
        if (dea.AnswerId == 0 && dea.NodeId == 0)
        {
            QuestTask task = QuestManager.GetTask("quest1", 0);
            task.IsDone = !task.IsDone;

        }
    }
}
