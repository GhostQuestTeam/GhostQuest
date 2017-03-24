
using UnityEngine;
using UnityEngine.UI;

//Отображает содержимое DialogNode на экран
namespace DialogueSystem 
{
	public class DialogueView : MonoBehaviour  {
		public GameObject answerPrefab;
		DialogueGraph dialogue;

		void Start(){
		}

		void Update(){
		}

		public DialogueGraph Dialogoue{
			get{
				return this.dialogue;
			}
			set{
				dialogue = value;
				_updateView ();
			}
		}

		private void  _updateView(){
            var answersPanel =transform.Find ("AnswersPanel");
            var invitation = transform.Find("Invitation Panel");

            invitation.GetComponentInChildren<Text>().text = dialogue.CurrentNode.Invitation;

            foreach (var answer in dialogue.CurrentNode.Answers) {
				var answerButton = (GameObject) Instantiate (answerPrefab);
                answerButton.GetComponentInChildren<Text>().text = answer.Message;
                answerButton.transform.SetParent (answersPanel);
			}
		}
    }
}
