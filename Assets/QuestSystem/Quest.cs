using System;

namespace QuestSystem
{
    [Serializable]
    public class Quest
    {
        private bool[] _isVisibleNote;
        private string[] _questNotes;
        private QuestTask[] _tasks;

        public QuestTask[] Tasks
        {
            get { return _tasks; }
            set { _tasks = value; }
        }

        public string[] QuestNotes
        {
            get { return _questNotes; }
            set { _questNotes = value; }
        }

        public void SetNoteVisible(uint i, bool isVisible)
        {
            _isVisibleNote[i] = isVisible;
        }

        public bool isNoteVisible(uint i)
        {
            return _isVisibleNote[i];
        }

        public Quest(QuestTask[] tasks, string[] questNotes)
        {
            Tasks = tasks;
            QuestNotes = questNotes;
        }
    }
}