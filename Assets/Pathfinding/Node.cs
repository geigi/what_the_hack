using UnityEngine;
using System.Collections;
using Utils;

namespace Pathfinding
{
	public class Node : IHeapItem<Node>
	{

		public Enums.TileState state;
		public Vector3 worldPosition;
		public Vector2Int gridPosition;
		public int gridX;
		public int gridY;
		public int movementPenalty;

		public int gCost;
		public int hCost;
		public Node parent;
		int heapIndex;

		public Node(Enums.TileState _state, Vector3 _worldPos, int _gridX, int _gridY)
		{
			state = _state;
			worldPosition = _worldPos;
			gridX = _gridX;
			gridY = _gridY;
			gridPosition = new Vector2Int(gridX, gridY);
			movementPenalty = 0;
		}

		public int fCost
		{
			get { return gCost + hCost; }
		}

		public int HeapIndex
		{
			get { return heapIndex; }
			set { heapIndex = value; }
		}

		public int CompareTo(Node nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(nodeToCompare.hCost);
			}

			return -compare;
		}
	}
}