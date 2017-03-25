namespace DialogueSystem
{
    public class DialogueAnswer
    {
        public string Message { get; }

        public int Next { get; }

        public DialogueAnswer(string message, int next)
        {
            this.Message = message;
            this.Next = next;
        }
    }
}