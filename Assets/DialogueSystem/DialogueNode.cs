using System.Linq;

namespace DialogueSystem
{
    public class DialogueNode
    {
        string invitation;
        DialogueAnswer[] answers;

        public DialogueAnswer[] Answers
        {
            get { return answers; }
        }

        public string Invitation
        {
            get { return invitation; }
        }

        public override bool Equals(object obj)
        {
            var node = obj as DialogueNode;
            if(node == null)
            {
                return false;
            }
            return Enumerable.SequenceEqual(Answers, node.Answers) && Invitation == node.Invitation;
        }

        public DialogueNode(string invitation, DialogueAnswer[] answers)
        {
            this.invitation = invitation;
            this.answers = answers;
        }
    }
}