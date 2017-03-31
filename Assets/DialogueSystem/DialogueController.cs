using UnityEngine;

// Прикрепляет к GameObjecty диалог
namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        public string dialoguePath;
        public GameObject dialoguePanel;
        private DialogueGraph dialogue;

        void Start()
        {
            string dialogueJson = Resources.Load<TextAsset>(dialoguePath).text;
            dialogue = DialogueParser.Parse(dialogueJson);
            dialoguePanel.SetActive(false);
        }

        void Update()
        {

        }

        public void onRay()
        {
            dialoguePanel.SetActive(true);
            TextAsset asset = Resources.Load<TextAsset>(dialoguePath);
            string dialogueJson = asset.text;
			dialogue = DialogueParser.Parse(dialogueJson);
            dialoguePanel.GetComponent<DialogueView>().Dialogoue = dialogue;
        }
    }
}