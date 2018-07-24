using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

namespace Pathfinding
{
	public class PathRequestManager : MonoBehaviour
	{

		Queue<PathResult> results = new Queue<PathResult>();

		static PathRequestManager instance;
		Pathfinding pathfinding;

		void Awake()
		{
			instance = this;
			pathfinding = GetComponent<Pathfinding>();
		}

		void Update()
		{
			if (results.Count > 0)
			{
				int itemsInQueue = results.Count;
				lock (results)
				{
					for (int i = 0; i < itemsInQueue; i++)
					{
						PathResult result = results.Dequeue();
						result.callback(result.path, result.success, result.EmployeeData);
					}
				}
			}
		}

		public static void RequestPath(PathRequest request, EmployeeData employeeData)
		{
			ThreadStart threadStart = delegate
			{
				instance.pathfinding.FindPath(request, instance.FinishedProcessingPath, employeeData);
			};
			threadStart.Invoke();
		}

		public void FinishedProcessingPath(PathResult result)
		{
			lock (results)
			{
				results.Enqueue(result);
			}
		}
	}

	public struct PathResult
	{
		public List<Node> path;
		public bool success;
		public EmployeeData EmployeeData;
		public Action<List<Node>, bool, EmployeeData> callback;

		public PathResult(List<Node> path, bool success, EmployeeData employeeData, Action<List<Node>, bool, EmployeeData> callback)
		{
			this.path = path;
			this.success = success;
			this.EmployeeData = employeeData;
			this.callback = callback;
		}
	}

	public struct PathRequest
	{
		public Vector2Int pathStart;
		public Vector2Int pathEnd;
		public Action<List<Node>, bool, EmployeeData> callback;

		public PathRequest(Vector2Int _start, Vector2Int _end, Action<List<Node>, bool, EmployeeData> _callback)
		{
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
}