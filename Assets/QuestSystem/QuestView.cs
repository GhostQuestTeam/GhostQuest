using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    //TODO Добавить кэширование
    // И обновление по событиям
    public class QuestView : MonoBehaviour
    {
        //public GameObject QuestPanel;
        public GameObject QuestTitlePrefab;
        public GameObject TaskCheckboxPrefab;
        public GameObject QuestNotePrefab;
        public GameObject UIPanel;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Close()
        {
            UIPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Draw()
        {
            var questList = transform.Find("Quests").Find("List");
            questList.Clear();
            foreach (var title in QuestManager.QuestTitles)
            {
                var questButton = (GameObject) Instantiate(QuestTitlePrefab);
                questButton.GetComponentInChildren<Text>().text = title;
                questButton.transform.SetParent(questList);

                var tmpTitle = new string(title.ToCharArray());
                questButton.GetComponent<Button>().onClick.AddListener(() => _drawQuest(tmpTitle));
            }
        }

        private void _drawQuest(string questTitle)
        {
            var questTitleView = transform.Find("QuestTitle");
            questTitleView.GetComponent<Text>().text = questTitle;

            var tasksList = transform.Find("TaskList");
            var notesList = transform.Find("QuestNotes");

            tasksList.Clear();
            notesList.Clear();

            foreach (var task in QuestManager.GetVisibleTasks(questTitle))
            {
                var taskCheckbox = Instantiate(TaskCheckboxPrefab);
                taskCheckbox.GetComponentInChildren<Text>().text = " " + task.Title;
                taskCheckbox.transform.SetParent(tasksList);
                if (!task.IsDone)
                {
                    taskCheckbox.transform.Find("Background/Checkmark").gameObject.SetActive(false);
                }
            }

            foreach (var note in QuestManager.GetVisibleNotes(questTitle))
            {
                var noteObject = Instantiate(QuestNotePrefab);
                noteObject.GetComponent<Text>().text =note + "\n";
                noteObject.transform.SetParent(notesList);
            }
        }
    }
}