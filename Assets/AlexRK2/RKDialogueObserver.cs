using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using QuestSystem;
using UnityEngine;

public class RKDialogueObserver : MonoBehaviour
{

    private const string _QUEST = "Example quest";
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
        if (dea.AnswerId == 0 && dea.DialogueId == 0)
        {
            QuestManager.StartQuest("rk_quest");
            QuestManager.ShowQuestNote(_QUEST, 0);
            QuestManager.ShowQuestNote(_QUEST, 1);
            QuestManager.ShowQuestNote(_QUEST, 2);

        }
        if (dea.AnswerId == 1 && dea.DialogueId == 0)
        {
            QuestTask task = QuestManager.GetTask(_QUEST, 0);
            task.IsVisible = true;
            StateControl.currentStep = StateControl.QUEST_STEP.FIRST_TALK_FINISHED;
            UnityEngine.SceneManagement.SceneManager.LoadScene (1);

        }
        if (dea.AnswerId == 2 && dea.DialogueId == 0)
        {
            QuestTask task = QuestManager.GetTask(_QUEST, 1);
            task.IsVisible = true;
            StateControl.currentStep = StateControl.QUEST_STEP.SECOND_TALK_FINISHED;
            UnityEngine.SceneManagement.SceneManager.LoadScene (1);
        }
        if (dea.AnswerId == 3 && dea.DialogueId == 0)
        {
            QuestTask task = QuestManager.GetTask(_QUEST, 0);
            task.IsVisible = true;
            StateControl.currentStep = StateControl.QUEST_STEP.THIRD_TALK_FINISHED;
            UnityEngine.SceneManagement.SceneManager.LoadScene (2);


        }
    }
}
