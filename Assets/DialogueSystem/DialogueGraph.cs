using System.Collections.Generic;

namespace DialogueSystem
{
    public class DialogueGraph
    {
        Dictionary<int, DialogueNode> _nodes;

        public DialogueNode CurrentNode
        {
            get { return _nodes[CurrentNodeId]; }
        }

        public int CurrentNodeId { get; set; } = 0;

        public void ChooseAnswer(uint index)
        {
            CurrentNodeId = CurrentNode.Answers[index].Next;
        }

        public DialogueGraph()
        {
            this._nodes = new Dictionary<int, DialogueNode>();
        }

        public DialogueGraph(Dictionary<int, DialogueNode> nodes)
        {
            this._nodes = nodes;
        }

        public void addNode(int index, DialogueNode node)
        {
            _nodes.Add(index, node);
        }
    }
}