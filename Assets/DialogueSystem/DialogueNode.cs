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

        public override string ToString()
        {
            var result = "{\n";
            result += "\tinvitation: " + Invitation + "\n";
            result += "\tanswers:[ ";
            foreach (var answer in answers)
            {
                result += answer.ToString().Replace("\n", "\n\t") + ",\n";
            }
            result += "\t]\n}";
            return result;
        }

        public DialogueNode(string invitation, DialogueAnswer[] answers)
        {
            this.invitation = invitation;
            this.answers = answers;
        }
    }
}