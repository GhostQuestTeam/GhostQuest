using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;


public class UiController : MonoBehaviour
{

    public GameObject QuestPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnQuestsClick()
    {
        QuestPanel.SetActive(true);
        QuestPanel.GetComponent<QuestView>().Draw();
        gameObject.SetActive(false);
    }
}
