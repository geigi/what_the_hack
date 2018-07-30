﻿using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

/// <summary>
/// This class represents an employee in the Game. All data is saved in the EmployeeData Scriptable Object.
/// All employee related logic is implemented here.
/// </summary>
public class Employee : MonoBehaviour {
	const float minPathUpdateTime = .2f;
	const float speed = 1.0f;
	
	public EmployeeData employeeData;

	private GameObject employeeLayer;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private AGrid grid;

	private Tilemap tilemap;

	private bool walking = false;
	private bool idle = true;
	private List<Node> path;
	
	void Start () {
		employeeLayer = GameObject.FindWithTag("EmployeeLayer");
		this.grid = GameObject.FindWithTag("Pathfinding").GetComponent<AGrid>();
		tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
		gameObject.transform.parent = employeeLayer.transform;
		var tile = grid.getRandomFreeNode();

		gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(tile.gridX, tile.gridY, 0));
	}

	/// <summary>
	/// This needs to be called before the employee is used.
	/// Fills the object with specific data.
	/// </summary>
	/// <param name="employeeData">Data for this employee.</param>
	public void init(EmployeeData employeeData)
	{
		this.employeeData = employeeData;

		// place the right animations
		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		this.animator = gameObject.AddComponent<Animator>();
		this.animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("EmployeeAnimations");
		var animatorOverrideController = new AnimatorOverrideController(this.animator.runtimeAnimatorController);
		animatorOverrideController["idle"] = employeeData.idleAnimation;
		animatorOverrideController["walking"] = employeeData.walkingAnimation;
		animatorOverrideController["working"] = employeeData.workingAnimation;
        //this.animator.runtimeAnimatorController = animatorOverrideController;
	}

	/// <summary>
	/// Start/Stop idle walking.
	/// </summary>
	/// <param name="start"></param>
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
		Debug.unityLogger.Log(LogType.Log, "Start: " + go.x.ToString() + ":" + go.y.ToString());
		Debug.unityLogger.Log(LogType.Log, "End: " + end.x.ToString() + ":" + end.y.ToString());
	}

	/// <summary>
	/// Follow the calculated path.
	/// </summary>
	/// <returns></returns>
	IEnumerator FollowPath() {
		bool followingPath = true;
		int pathIndex = 0;
		this.walking = true;
		this.animator.SetTrigger("walking");

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
				if (step.x < 0) {
					spriteRenderer.flipX = false;
				}
				else if (step.x > 0) {
					spriteRenderer.flipX = true;
				}

				transform.Translate(step);
				yield return null;
			}
		}

		this.animator.SetTrigger("idle");
		yield return new WaitForSeconds(4);
		walking = false;
	}
	
	/// <summary>
	/// Get's called when the PathRequester found a path for this employee.
	/// </summary>
	/// <param name="path"></param>
	/// <param name="success"></param>
	private void PathFound(List<Node> path, bool success)
	{
		if (success)
		{
			this.path = path;
			foreach (var node in path)
			{
				Debug.unityLogger.Log(LogType.Log, "Node: " + node.gridX.ToString() + ":" + node.gridY.ToString());
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
