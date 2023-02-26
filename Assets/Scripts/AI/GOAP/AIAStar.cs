public enum AStarNodeState
{
	Unchecked,
	Open,
	Closed,
	Unpassable
}

public class AStarNode
{
	public int ID = -1;

	public float goal = 0;
	public float heuristic = 0;
	public float fitness = float.MaxValue;

	public AStarNode parent = null;
	public AStarNodeState nodeState = AStarNodeState.Unchecked;

	public AIAStar astar;

	public WS worldStateCur = new WS();
	public WS worldStateGoal = new WS();

	public AStarNode()
	{
		worldStateCur.ResetWS();
		worldStateGoal.ResetWS();
	}
	public virtual void SetState(AStarNodeState newState)
	{
		nodeState = newState;
	}
}

public class AIAStar
{
	AIAStarNodeStorage storage;
	AIAStarGoal goal;
	AIAStarMap map;

	AStarNode nodeCur;

	int nodeSource = -1;

	public AStarNode GetNodeCur() => nodeCur;
	public AIAStarNodeStorage GetStorage() => storage;
	public AIAStarMap GetMap() => map;

	public void Init(AIAStarNodeStorage storage, AIAStarGoal goal, AIAStarMap map)
	{
		this.storage = storage;
		this.goal = goal;
		this.map = map;
	}

	public void SetAStarSource(int nodeSource)
	{
		storage.ResetStorage();
		this.nodeSource = nodeSource;
	}

	public void RunAStar(AICharacter AI)
	{
		int neighbors, neighbor;
		AStarNode nodeNeighbor;
		int neighborNode;
		float g, h, f;

		nodeCur = storage.CreateNode(nodeSource);
		if (nodeCur == null)
		{
			return;
		}
		storage.AddToOpenList(nodeCur);

		h = goal.GetHeuristicDistance(nodeCur);
		nodeCur.goal = 0f;
		nodeCur.heuristic = h;
		nodeCur.fitness = h;

		while (true)
		{
			nodeCur = storage.RemoveCheapestOpenNode();

			if (nodeCur != null)
				storage.AddToClosedList(nodeCur);

			if (goal.IsAStarFinished(nodeCur))
				break;

			neighbors = map.GetNumNeighbors(AI, nodeCur);
			for (neighbor = 0; neighbor < neighbors; ++neighbor)
			{
				neighborNode = map.GetNeighbor(neighbor);

				if (neighborNode == -1)
					continue;
				
				nodeNeighbor = storage.CreateNode(neighborNode);

				if (nodeNeighbor.Equals(nodeCur.parent))
					continue;

				g = nodeCur.goal;
				g += goal.GetActualCost(nodeCur, nodeNeighbor);
				h = goal.GetHeuristicDistance(nodeNeighbor);
				f = g + h;

				const float epsilon = 1e-6f;
				if (f >= nodeNeighbor.fitness * (1f - epsilon))
				{
					continue;
				}

				nodeNeighbor.goal = g;
				nodeNeighbor.heuristic = h;
				nodeNeighbor.fitness = g + h;

				storage.AddToOpenList(nodeNeighbor);
				nodeNeighbor.parent = nodeCur;
			}
		}
	}
}
