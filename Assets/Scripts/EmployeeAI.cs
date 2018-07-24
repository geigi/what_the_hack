using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEngine;

public class EmployeeAI : MonoBehaviour {
	public EmployeeData EmployeeData;

	// Use this for initialization
	void Start () {
		WalkEmployee();
	}

	void WalkEmployee() {
		var callback = new System.Action<List<Node>, bool, EmployeeData>(Target);
		Pathfinding.PathRequestManager.RequestPath(new Pathfinding.PathRequest(new Vector2Int(4,4), new Vector2Int(8,8), callback), EmployeeData);
	}

	private void Target(List<Node> path, bool success, EmployeeData employeeData)
	{
		Debug.Log(success);
		Debug.Log(path);
		Debug.Log(employeeData.name);
	}
	
	IEnumerator FollowPath(List<Node> path, EmployeeData employeeData) {

		bool followingPath = true;
		int pathIndex = 0;

		float speedPercent = 1;

		while (followingPath)
		{
			float step = speedPercent * Time.deltaTime;
			
			yield return null;

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
