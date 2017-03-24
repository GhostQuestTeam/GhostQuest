using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class DialogueGraph
{
    Dictionary<int, DialogueNode> nodes;
    int _currentNodeId = 0;

    public DialogueNode CurrentNode{
        get { return nodes[_currentNodeId]; }
    }

	public int CurrentNodeId{
		get { return _currentNodeId; }
        set { _currentNodeId = value; }
	}

    public void ChooseAnswer(uint index){
        _currentNodeId = CurrentNode.Answers[index].Next;
    }

    public DialogueGraph(){
        this.nodes = new Dictionary<int, DialogueNode>();
    }

    public DialogueGraph(Dictionary<int, DialogueNode> nodes){
        this.nodes = nodes;
    }

    public void addNode(int index, DialogueNode node){
        nodes.Add(index, node);
    }
}