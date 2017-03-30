using System;

namespace DialogueSystem
{
    public class DialogueAnswer
    {

        private string _message;
        private int _next;
        private Func<bool> condition;

        public string Message {
			get { return _message;}
        }

        public int Next
        {
            get { return _next; }
        }

        public DialogueAnswer(string message, int next)
        {
            this._message = message;
            this._next = next;
        }
    }
}