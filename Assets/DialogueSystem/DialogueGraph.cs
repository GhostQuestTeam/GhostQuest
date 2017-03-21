using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGraph  {
    Dictionary<int, DialogueNode> nodes;
    int currentNodeId = 0;

    public DialogueNode CurrentNode
    {
        get
        {
            return nodes[currentNodeId];
        }
    }

    public void ChooseAnswer(uint index)
    {
           currentNodeId = CurrentNode.Answers[index].Next;
    }
    
    public DialogueGraph(Dictionary<int, DialogueNode> nodes)
    {
        this.nodes = nodes;
    }
}
