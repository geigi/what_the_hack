using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Utils.Enums;

public class TileMap : MonoBehaviour {
    public Tilemap tileMap;
    private GridInformation gridInformation;
	private TileBase[,] map;

	// Use this for initialization
	void Start ()
	{
		
		this.map = new TileBase[13,13];

		this.gridInformation = this.tileMap.GetComponent<GridInformation>();
        for (int x = 0; x < 13; x++) {
            for (int y = 0; y < 13; y++)
            {
	            var pos = new Vector3Int(x, y, 0);
				this.map[x,y] = this.tileMap.GetTile(pos);
	            
            }
        }
	}
	
	/// <summary>
	/// Sets the state of this tile.
	/// </summary>
	/// <param name="state">State.</param>
	/// <param name="obj">Object occuping this state. Pass null if state is FREE.</param>
	public void setState(Vector3Int pos, TileState state, GameObject obj)
	{
		this.gridInformation.SetPositionProperty(pos, "state", (int)TileState.DISABLED);
		
		if (obj != null)
		{
			this.gridInformation.SetPositionProperty(pos, "occupiedBy", obj);
		}
		else
		{
			
		}
	}

	/// <summary>
	/// Gets the state.
	/// </summary>
	/// <returns>The state.</returns>
	public TileState getState(Vector3Int pos)
	{
		return (TileState)this.gridInformation.GetPositionProperty(pos, "state", (int)TileState.DISABLED);
	}

	/// <summary>
	/// Gets the occupying object.
	/// </summary>
	/// <returns>The occupying object. Returns null if tile is not occupied.</returns>
	public GameObject getOccupyingObject(Vector3Int pos)
	{
		var obj = this.gridInformation.GetPositionProperty(pos, "state", new GameObject("notOccupied"));
		if (obj.name != "notOccupied")
		{
			return obj;
		}
		else
		{
			return null;
		}
	}
	
	public IReadOnlyList<Vector3Int> findPath(Vector3Int start, Vector3Int end) {

		return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
