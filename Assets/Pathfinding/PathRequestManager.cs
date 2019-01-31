using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using Object = System.Object;

namespace Pathfinding
{
	/// <summary>
	/// Use this manager to request and receive paths between two tiles.
	/// </summary>
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
						result.callback(result.path, result.success, result.parameter);
					}
				}
			}
		}

		/// <summary>
		/// Requests a path from a starting to and endpoint on the tilemap.
		/// </summary>
		/// <param name="request">Path Request object.</param>
		public static void RequestPath(PathRequest request)
		{
			ThreadStart threadStart = delegate
			{
				instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
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

	/// <summary>
	/// Contains the pathfinding results.
	/// </summary>
	public struct PathResult
	{
		public List<Node> path;
		public bool success;
		public object parameter;
		public Action<List<Node>, bool, object> callback;

		/// <summary>
		/// Initialize a PathResult.
		/// </summary>
		/// <param name="path">List of Nodes which are the calculated path.</param>
		/// <param name="success">Was a path found?</param>
		/// <param name="callback">Callback object.</param>
		public PathResult(List<Node> path, bool success, object parameter, Action<List<Node>, bool, object> callback)
		{
			this.path = path;
			this.success = success;
			this.callback = callback;
			this.parameter = parameter;
		}
	}

	/// <summary>
	/// A struct containing all necessary information for a path request.
	/// </summary>
	public struct PathRequest
	{
		public Vector2Int pathStart;
		public Vector2Int pathEnd;
		public object parameter;
		public Action<List<Node>, bool, object> callback;

		/// <summary>
		/// Initialize a PathRequest.
		/// </summary>
		/// <param name="_start">Start coordinates.</param>
		/// <param name="_end">End coordinates.</param>
		/// <param name="_callback">Callback object. Get's called when the pathfinding is done.</param>
		public PathRequest(Vector2Int _start, Vector2Int _end, object parameter, Action<List<Node>, bool, object> _callback)
		{
			pathStart = _start;
			pathEnd = _end;
			this.parameter = parameter;
			callback = _callback;
		}
	}
}