
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
            foreach (Transform child in answersPanel)
            {
                GameObject.Destroy(child.gameObject);
            }

            var invitation = transform.Find("Invitation Panel");
           

            invitation.GetComponentInChildren<Text>().text = dialogue.CurrentNode.Invitation;

            for (uint i=0; i <dialogue.CurrentNode.Answers.Length; i++) {
                
                var answer = dialogue.CurrentNode.Answers[i];
                var answerButton = (GameObject) Instantiate (answerPrefab);
                answerButton.GetComponentInChildren<Text>().text = answer.Message;
                answerButton.transform.SetParent (answersPanel);

                uint tmp = i;
                answerButton.GetComponent<Button>().onClick.AddListener(
                    () => {
                        dialogue.ChooseAnswer(tmp);
                        _updateView();
                    }
                );
            }
            
		}
    }
}
