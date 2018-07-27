using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

public class Employee : MonoBehaviour {
	const float minPathUpdateTime = .2f;
	const float speed = 1.0f;
	
	public EmployeeData employeeData;

	private GameObject employeeLayer;
	private SpriteRenderer renderer;

	private AGrid grid;

	private Tilemap tilemap;

	private bool walking = false;
	private bool idle = true;
	private List<Node> path;
	// Use this for initialization
	void Start () {
		renderer = gameObject.AddComponent<SpriteRenderer>();
		renderer.sprite = employeeData.sprite;
		employeeLayer = GameObject.FindWithTag("EmployeeLayer");
		this.grid = GameObject.FindWithTag("Pathfinding").GetComponent<AGrid>();
		tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
		gameObject.transform.parent = employeeLayer.transform;
		var tile = grid.getRandomFreeNode();

		gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(tile.gridX, tile.gridY, 0));
	}

	public void init(EmployeeData employeeData)
	{
		this.employeeData = employeeData;
	}

	public void idleWalking(bool start)
	{
		idle = start;
		if (!start & walking)
		{
			walking = false;
		}
	}

	private void RequestNewIdleWalk()
	{
		var callback = new System.Action<List<Node>, bool>(PathFound);
		var position = gameObject.transform.position;
		var go = this.grid.getNode(position).gridPosition;
		var end = this.grid.getRandomFreeNode().gridPosition;
		Pathfinding.PathRequestManager.RequestPath(new Pathfinding.PathRequest(go, end, callback));
		Debug.Log("Start: " + go.x.ToString() + ":" + go.y.ToString());
		Debug.Log("End: " + end.x.ToString() + ":" + end.y.ToString());
	}

	IEnumerator FollowPath() {
		bool followingPath = true;
		int pathIndex = 0;
		this.walking = true;

		while (followingPath & walking)
		{
			var stepLength = speed * Time.deltaTime;
			var step = (path[pathIndex].worldPosition - transform.position).normalized * stepLength;
			var distance = Vector3.Distance(transform.position, path[pathIndex].worldPosition);

			while (stepLength > distance & followingPath)
			{
				if (pathIndex == path.Count - 1)
				{
					transform.position = path[pathIndex].worldPosition;
					followingPath = false;
				}
				else
				{
					step = (path[pathIndex].worldPosition - transform.position).normalized * distance;
					transform.Translate(step);
					stepLength -= distance;
					pathIndex++;
					distance = Vector3.Distance(transform.position, path[pathIndex].worldPosition);
					step = (path[pathIndex].worldPosition - transform.position).normalized * stepLength;
				}
			}

			if (followingPath)
			{
				transform.Translate(step);
				yield return null;
			}
		}

		yield return new WaitForSeconds(4);
		walking = false;
	}
	
	private void PathFound(List<Node> path, bool success)
	{
		if (success)
		{
			this.path = path;
			foreach (var node in path)
			{
				Debug.Log("Node: " + node.gridX.ToString() + ":" + node.gridY.ToString());
			}
			StopCoroutine(nameof(FollowPath));
			StartCoroutine(nameof(FollowPath));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (idle & !walking)
		{
			RequestNewIdleWalk();
		}
	}
}
