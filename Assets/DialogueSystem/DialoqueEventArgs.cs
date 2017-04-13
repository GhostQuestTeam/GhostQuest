using System;

namespace DialogueSystem
{
    public class DialogueEventArgs : EventArgs
    {
        public string DialogueId { get; set; }
        public int NodeId { get; set; }
        public uint AnswerId { get; set; }

        public DialogueEventArgs(string dialogueId ,int nodeId, uint answerId)
        {
            DialogueId = dialogueId;
            nodeId = nodeId;
            AnswerId = answerId;
        }
    }
}