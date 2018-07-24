using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using Utils;

namespace Pathfinding{
public class Pathfinding : MonoBehaviour {

	AGrid _aGrid;
	
	void Awake() {
		_aGrid = GetComponent<AGrid>();
	}
	

	public void FindPath(PathRequest request, Action<PathResult> callback, EmployeeData employeeData) {
		
		Stopwatch sw = new Stopwatch();
		sw.Start();
		
		List<Node> waypoints = new List<Node>();
		bool pathSuccess = false;
		
		Node startNode = _aGrid.getNode(request.pathStart);
		Node targetNode = _aGrid.getNode(request.pathEnd);
		startNode.parent = startNode;
		
		
		if (targetNode.state == Enums.TileState.FREE) {
			Heap<Node> openSet = new Heap<Node>(_aGrid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				
				if (currentNode == targetNode) {
					sw.Stop();
					//print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					pathSuccess = true;
					break;
				}
				
				foreach (Node neighbour in _aGrid.GetNeighbours(currentNode)) {
					if (neighbour.state != Enums.TileState.FREE || closedSet.Contains(neighbour)) {
						continue;
					}
					
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else 
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}
		if (pathSuccess) {
			waypoints = RetracePath(startNode,targetNode);
			pathSuccess = waypoints.Count > 0;
		}
		callback (new PathResult (waypoints, pathSuccess, employeeData, request.callback));
	}
		
	
	List<Node> RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse();
		return path;
	}
	
	Vector2Int[] SimplifyPath(List<Node> path) {
		List<Vector2Int> waypoints = new List<Vector2Int>();
		Vector2Int directionOld = Vector2Int.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2Int directionNew = new Vector2Int(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
	
	
}
}