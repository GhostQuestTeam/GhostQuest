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

        public static void ShowTask(string questTitle, uint taskId)
        {
            _quests[questTitle].Tasks[taskId].IsVisible = true;
        }

        public static void DoTask(string questTitle, uint taskId)
        {
            _quests[questTitle].Tasks[taskId].IsDone = true;
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

        private static IEnumerable<string> QuestTitles
        {
            get { return _quests.Values.Select(quest => quest.Title); }
        }

    }
}