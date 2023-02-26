using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAStarNodeStorage
{
	LinkedList<AStarNode> openList = new LinkedList<AStarNode>();
	LinkedList<AStarNode> closedList = new LinkedList<AStarNode>();

	public AIAStar astar;

    public void Init(AIAStar newAStar) => astar = newAStar;

    public AStarNode CreateNode(int nodeID)
	{
		AStarNode node = new AStarNode();
		node.ID = nodeID;
		node.astar = astar;

		return node;
	}

	public void DestroyNode(AStarNode node)
	{
	}

	public void ResetStorage()
	{
		ClearList(openList);
		ClearList(closedList);
	}

	void ClearList(LinkedList<AStarNode> list)
	{
		list?.Clear();
	}

	public void AddToOpenList(AStarNode node)
	{
		AStarNodeState state = node.nodeState;

		if (state == AStarNodeState.Open)
		{
			return;
		}

		AddToList(openList, node);
		node.SetState(AStarNodeState.Open);
	}

	public void AddToClosedList(AStarNode node)
	{
		AddToList(closedList, node);
		node.SetState(AStarNodeState.Closed);
	}

	void AddToList(LinkedList<AStarNode> list, AStarNode node)
	{
		list?.AddFirst(node);
	}

	public void RemoveFromOpenList(AStarNode node)
	{
		RemoveFromList(openList, node);
	}

	public void RemoveFromClosedList(AStarNode node)
	{
		RemoveFromList(closedList, node);
	}

	void RemoveFromList(LinkedList<AStarNode> list, AStarNode node)
	{
		list?.Remove(node);
	}

	public AStarNode RemoveCheapestOpenNode()
	{
		LinkedListNode<AStarNode> nodeCheapest = openList.First;

		var pNode = openList.First;
		AStarNode nodeVal = null;

		if (openList.First != null)
			nodeVal = openList.First.Value;

		while (pNode != null)
		{
			if (pNode.Value.fitness < nodeCheapest.Value.fitness)
			{
				nodeCheapest = pNode;
				nodeVal = pNode.Value;
			}
			pNode = pNode.Next;
		}

		if (nodeCheapest != null)
		{
			RemoveFromOpenList(nodeCheapest.Value);
		}

		return nodeVal;
	}

	public AStarNode FindInOpenList(int node)
	{
		return FindInList(openList, node);
	}

	public AStarNode FindInClosedList(int node)
	{
		return FindInList(closedList, node);
	}

	AStarNode FindInList(LinkedList<AStarNode> list, int nodeID)
	{
		var node = openList.First;

		while (node != null)
		{
			if (node.Value.ID == nodeID)
			{
				return node.Value;
			}
			node = node.Next;
		}

		return null;
	}

}
