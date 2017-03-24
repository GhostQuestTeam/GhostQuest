using UnityEngine;

// Прикрепляет к GameObjecty диалог
namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour {

		public string dialoguePath;
		public GameObject dialoguePanel;
		private	DialogueGraph dialogue;

        void Start () {
            string dialogueJson = Resources.Load<TextAsset>(dialoguePath).text;
            dialogue = DialogueParser.Parse (dialogueJson);
            dialoguePanel.SetActive(false);
			//dialogue = DialogueParser.Parse("{\"id\":0, \"invitation\":\"text\", \"answers\":[{\"message\":\"test\"}] }");
           

			//GameObject answer = (GameObject)Instantiate(answerPrefab);
			//answer.transform.SetParent (canvas.transform);
        }
	
        void Update () {
			
        }

        public void onRay(){
            dialoguePanel.SetActive(true);
            dialoguePanel.GetComponent<DialogueView>().Dialogoue = dialogue;
        }
    }
}
