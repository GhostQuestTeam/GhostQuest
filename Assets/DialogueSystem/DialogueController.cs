using UnityEngine;

// Прикрепляет к GameObjecty диалог
namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        public string dialoguePath;
        private GameObject _dialoguePanel;
        private DialogueGraph dialogue;

        void Start()
        {
            var uiControllerObject = GameObject.FindWithTag("UIController");
            _dialoguePanel = uiControllerObject.GetComponent<UIController>().DialoguePanel;
            string dialogueJson = Resources.Load<TextAsset>(dialoguePath).text;
            dialogue = DialogueParser.Parse(dialogueJson);
        }

        void Update()
        {
        }

        public void onRay()
        {
            _dialoguePanel.SetActive(true);
            _dialoguePanel.GetComponent<DialogueView>().Dialogoue = dialogue;
        }
    }
}