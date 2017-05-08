using System;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    //TODO Добавить кэширование
    // И обновление по событиям
    public class QuestView : MonoBehaviour
    {

        public const string DEFAULT_QUEST_TITLE_PREFAB = "QuestsUI/QuestTitleButton";
        public const string DEFAULT_QUEST_NOTE_PREFAB = "QuestsUI/QuestNote";
        public const string DEFAULT_TASK_CHECKBOX_PREFAB = "QuestsUI/TaskCheckbox";

        public string QuestTitlePrefabPath = DEFAULT_QUEST_TITLE_PREFAB;
        public string QuestNotePrefabPath = DEFAULT_QUEST_NOTE_PREFAB;
        public string TaskChecckboxPrefabPath = DEFAULT_TASK_CHECKBOX_PREFAB;

        public event Action OnClose;

        private GameObject questTitlePrefab;
        private GameObject taskCheckboxPrefab;
        private GameObject questNotePrefab;

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
            gameObject.SetActive(false);
            if(OnClose != null){
                OnClose();
            }
        }

        public void Draw()
        {
            questTitlePrefab =    Resources.Load(QuestTitlePrefabPath) as GameObject;
            questNotePrefab =     Resources.Load(QuestNotePrefabPath) as GameObject;
            taskCheckboxPrefab =  Resources.Load(TaskChecckboxPrefabPath) as GameObject;

            var questList = transform.Find("QuestWindow/BodyPanel/QuestsList");
            questList.Clear();
            foreach (var title in QuestManager.QuestTitles)
            {
                var questButton = (GameObject) Instantiate(questTitlePrefab);
                questButton.GetComponentInChildren<Text>().text = title;
                questButton.transform.SetParent(questList);

                var tmpTitle = new string(title.ToCharArray());
                questButton.GetComponent<Button>().onClick.AddListener(() => _drawQuest(tmpTitle));
            }
        }

        private void _drawQuest(string questTitle)
        {
            var questTitleView = transform.Find("QuestWindow/HeadPanel/TextQuestTitle");
            questTitleView.GetComponent<Text>().text = questTitle;

            var tasksList = transform.Find("QuestWindow/BodyPanel/TaskList");
            var notesList = transform.Find("QuestWindow/BodyPanel/QuestNotes");

            tasksList.Clear();
            notesList.Clear();

            foreach (var task in QuestManager.GetVisibleTasks(questTitle))
            {
                var taskCheckbox = Instantiate(taskCheckboxPrefab);
                taskCheckbox.GetComponentInChildren<Text>().text = " " + task.Title;
                taskCheckbox.transform.SetParent(tasksList);
                if (!task.IsDone)
                {
                    taskCheckbox.transform.Find("Background/Checkmark").gameObject.SetActive(false);
                }
            }

            foreach (var note in QuestManager.GetVisibleNotes(questTitle))
            {
                var noteObject = Instantiate(questNotePrefab);
                noteObject.GetComponent<Text>().text =note + "\n";
                noteObject.transform.SetParent(notesList);
            }
        }
    }
}