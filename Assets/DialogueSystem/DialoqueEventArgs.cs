using System;

namespace DialogueSystem
{
    public class DialogueEventArgs : EventArgs
    {
        public int DialogueId { get; set; }
        public uint AnswerId { get; set; }

        public DialogueEventArgs(int diaalogueId, uint answerId)
        {
            DialogueId = diaalogueId;
            AnswerId = answerId;
        }
    }
}