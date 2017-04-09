using System.Collections.Generic;
using System.Linq;

namespace DialogueSystem
{
    public class DialogueGraph
    {
        private Dictionary<int, DialogueNode> _nodes;
        private int _currentNodeId = 0;

        public DialogueNode CurrentNode
        {
            get { return _nodes[CurrentNodeId]; }
        }

        public int CurrentNodeId
        {
            get { return _currentNodeId; }
            set { _currentNodeId = value; }
        }

        public override bool Equals(object obj)
        {
            var dialogue = obj as DialogueGraph;
            if(dialogue == null)
            {
                return false;
            }
            return Enumerable.SequenceEqual(_nodes, dialogue._nodes) && CurrentNodeId == dialogue.CurrentNodeId;
        }

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