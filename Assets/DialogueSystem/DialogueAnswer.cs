using System;

namespace DialogueSystem
{
    public class DialogueAnswer
    {

        private string _message;
        private int _next;
        private Func<bool> _condition;

        public string Message {
			get { return _message;}
        }

        public int Next
        {
            get { return _next; }
        }

        public bool IsVisible
        {
            get { return _condition(); }
        }

        public void SetCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public override bool Equals(object obj)
        {
            var answer = obj as DialogueAnswer;
            if(answer == null)
            {
                return false;
            }
            return Message == answer.Message && Next == answer.Next;
        }

        public override string ToString()
        {
            var result = "\n{\n";
            result += "\tmessage: " + Message + "\n";
            result += "\tnext: " + Next + "\n";
            result += "}";
            return result;
        }

        public DialogueAnswer(string message, int next)
        {
            this._message = message;
            this._next = next;
            this._condition = () => true;
        }
    }
}