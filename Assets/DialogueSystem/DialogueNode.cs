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

        public DialogueNode(string invitation, DialogueAnswer[] answers)
        {
            this.invitation = invitation;
            this.answers = answers;
        }
    }
}