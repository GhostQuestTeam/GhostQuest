using System;

namespace QuestSystem
{
    [Serializable]
    public class QuestTask
    {
        private bool _done = false;
        private string _title;
        private bool _visible = true;

        public bool IsDone
        {
            get { return _done; }
            set { _done = value; }
        }

        public bool IsVisible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public string Title
        {
            get { return _title; }
        }

        public QuestTask(string title)
        {
            _title = title;
        }

        public QuestTask(string title, bool visible)
        {
            _title = title;
            IsVisible = visible;
        }

        public QuestTask(string title, bool visible, bool isDone)
        {
            _title = title;
            IsVisible = visible;
            IsDone = isDone;
        }
    }
}