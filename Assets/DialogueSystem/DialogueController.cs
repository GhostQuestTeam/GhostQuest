using UnityEngine;

// Прикрепляет к GameObjecty диалог
namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour {

		public string dialoguePath;
		public GameObject dialoguePanel;
		private	DialogueGraph dialogue;


        // Use this for initialization
        void Start () {
            string dialogueJson = Resources.Load<TextAsset>(dialoguePath).text;
            dialogue = DialogueParser.Parse (dialogueJson);
            dialoguePanel.SetActive(false);
			//dialogue = DialogueParser.Parse("{\"id\":0, \"invitation\":\"text\", \"answers\":[{\"message\":\"test\"}] }");
            dialoguePanel.SetActive(true);
			dialoguePanel.GetComponent<DialogueView> ().Dialogoue = dialogue;

			//GameObject answer = (GameObject)Instantiate(answerPrefab);
			//answer.transform.SetParent (canvas.transform);
        }
	
        // Update is called once per frame
        void Update () {
			
        }
    }
}
