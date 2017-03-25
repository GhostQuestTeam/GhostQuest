using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    public class Quest
    {
        private bool[] _isVisibleNote;

        public QuestTask[] Tasks { get; set; }

        public string[] QuestNotes { get; set; }

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