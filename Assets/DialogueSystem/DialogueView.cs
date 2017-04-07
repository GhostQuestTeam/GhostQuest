using System;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

//Отображает содержимое DialogNode на экран
namespace DialogueSystem
{
    public class DialogueView : MonoBehaviour
    {
        public const string DEFAULT_ANSWER_PREFAB = "DialoguesUI/DialogueAnswer";
        public string AnswerPrefabPath = DEFAULT_ANSWER_PREFAB;

        public event EventHandler<DialogueEventArgs> OnAnswerChoose;


        private GameObject answerPrefab;

        DialogueGraph dialogue;

        void Start()
        {
        }

        void Update()
        {
        }

        public DialogueGraph Dialogoue
        {
            get { return this.dialogue; }
            set
            {
                dialogue = value;
                _updateView();
            }
        }

        private void _updateView()
        {
            //TODO Почему то если присваивать в Start(), в этом методе значение сбрасывается на null
            answerPrefab = Resources.Load(AnswerPrefabPath) as GameObject;

            var answersPanel = transform.Find("AnswersPanel");
            answersPanel.Clear();
            var invitation = transform.Find("Invitation Panel");


            invitation.GetComponentInChildren<Text>().text = dialogue.CurrentNode.Invitation;

            for (uint i = 0; i < dialogue.CurrentNode.Answers.Length; i++)
            {
                var answer = dialogue.CurrentNode.Answers[i];
                if (!answer.IsVisible) continue;

                var answerButton = (GameObject) Instantiate(answerPrefab);
                answerButton.GetComponentInChildren<Text>().text = answer.Message;
                answerButton.transform.SetParent(answersPanel);

                uint tmp = i;
                answerButton.GetComponent<Button>()
                    .onClick.AddListener(
                        () =>
                        {
                            EventHandler<DialogueEventArgs> handler = OnAnswerChoose;
                            if (handler != null)
                            {
                                handler(this, new DialogueEventArgs(dialogue.CurrentNodeId, tmp));
                            }

                            int next = dialogue.CurrentNode.Answers[tmp].Next;
                            if (next != -1)
                            {
                                dialogue.CurrentNodeId = next;
                                _updateView();
                            }
                            else
                            {
                                gameObject.SetActive(false);
                            }
                        }
                    );
            }
        }
    }
}