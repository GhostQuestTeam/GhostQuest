namespace QuestSystem
{
    public class QuestTask
    {
        public bool IsDone { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public string Title { get; }

        public QuestTask(string title)
        {
            Title = title;
        }

        public QuestTask(string title, bool isVisible)
        {
            Title = title;
            IsVisible = isVisible;
        }

        public QuestTask(string title, bool isVisible, bool isDone)
        {
            Title = title;
            IsVisible = isVisible;
            IsDone = isDone;
        }
    }
}