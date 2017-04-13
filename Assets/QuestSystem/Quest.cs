using System;
using System.Linq;

namespace QuestSystem
{
    [Serializable]
    public class Quest
    {
        private string _title;
        private bool[] _isVisibleNote;
        private string[] _questNotes;
        private QuestTask[] _tasks;

        public string Title
        {
            get { return _title; }
        }

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

        public override bool Equals(object other)
        {
            var otherQuest = other as Quest;
            if (otherQuest == null) return false;

            return Title == otherQuest.Title &&
                   _isVisibleNote.SequenceEqual(otherQuest._isVisibleNote) &&
                   QuestNotes.SequenceEqual(otherQuest.QuestNotes) &&
                   Tasks.SequenceEqual(otherQuest.Tasks)
            ;
        }

        //TODO Использовать какое-нибудь готовое решение для сериализации в JSON
        public override string ToString()
        {
            var result = "{\n";
            result += "\ttitle: " + Title + "\n";
            result += "\tvisible_notes: [ ";
            foreach (var is_visible in _isVisibleNote)
            {
                result += is_visible.ToString() + ", ";
            }
            result += "\t]\n";

            result += "\tnotes: [ ";
            foreach (var note in QuestNotes)
            {
                result += note + ", ";
            }
            result += "\t]\n";

            result += "\ttasks: [";
            foreach (var task in Tasks)
            {
                result += task.ToString().Replace("\n", "\n\t") + ",";
            }
            result += "\n\t]\n";
            result += "}";
            return result;
        }

        public Quest(string title ,QuestTask[] tasks, string[] questNotes)
        {
            _title = title;
            _isVisibleNote = new bool[questNotes.Length];
            Tasks = tasks;
            QuestNotes = questNotes;
        }
    }
}