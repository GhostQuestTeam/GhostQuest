using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace QuestSystem
{
    public class QuestManager
    {

        private const string _QUEST_FOLDER = "Quests/";

        private static Dictionary<string, Quest> _quests;

        public static void StartQuest(string questTitle)
        {
            string questJson = Resources.Load<TextAsset>(_QUEST_FOLDER + questTitle).text;
            var quest = QuestParser.Parse(questJson);
            _quests.Add(quest.Title, quest);
        }

        public static void ShowQuestNote(string questTitle, uint noteId)
        {
            _quests[questTitle].SetNoteVisible(noteId, true);
        }

        public static QuestTask GetTask(string questTitle, uint taskId)
        {
            return _quests[questTitle].Tasks[taskId];
        }

        public static QuestTask[] GetTasks(string questTitle)
        {
            return _quests[questTitle].Tasks;
        }

        public static void ShowTask(string questTitle, uint taskId)
        {
            _quests[questTitle].Tasks[taskId].IsVisible = true;
        }

        public static void DoTask(string questTitle, uint taskId)
        {
            _quests[questTitle].Tasks[taskId].IsDone = true;
        }

        public static IEnumerable<QuestTask> GetVisibleTasks(string questTitle)
        {
            return _quests[questTitle].Tasks.Where((task) => task.IsVisible);
        }

        public static IEnumerable<string> GetVisibleNotes(string questTitle)
        {
            var quest = _quests[questTitle];
            for (uint i = 0; i < quest.QuestNotes.Length; i++)
            {
                if (quest.isNoteVisible(i))
                {
                    yield return quest.QuestNotes[i];
                }
            }
        }

        public static IEnumerable<string> QuestTitles
        {
            get { return _quests.Values.Select(quest => quest.Title); }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void DownloadQuests() //TODO В дальнеейшем будут читаться сохраннённые квесты с диска
        {
            _quests = new Dictionary<string, Quest>();
            string questJson1 =
                "{\"title\":\"quest1\", \"tasks\":[{\"description\":\"First test task\", \"visible\":true}], \"notes\":[\"note1\", \"note2\"] }";
            string questJson2 =
                "{\"title\":\"quest2\", \"tasks\":[{\"description\":\"Second test task\", \"visible\":true}], \"notes\":[\"note1\", \"note2\"] }";

            var quest1 = QuestParser.Parse(questJson1);
            var quest2 = QuestParser.Parse(questJson2);

            _quests.Add(quest1.Title, quest1);
            _quests.Add(quest2.Title, quest2);

            ShowQuestNote(quest1.Title, 0);
            ShowQuestNote(quest1.Title, 1);

        }

    }
}