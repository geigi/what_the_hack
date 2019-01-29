using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Random = UnityEngine.Random;

namespace Pathfinding
{
  public class AGrid : MonoBehaviour
  {
    public Tilemap tilemap;
    public Grid go_grid;
    
    Node[,] grid;

    public const int GridSizeX = 13, GridSizeY = 13;
    public const int SORTING_ORDER_GRANULARITY = 20;

    private IReadOnlyList<Vector2Int> disabledTiles;

    void Awake()
    {
      CreateGrid();
    }

    public int MaxSize
    {
      get { return GridSizeX * GridSizeY; }
    }

    void CreateGrid()
    {
      grid = new Node[GridSizeX, GridSizeY];

      var disabled = new List<Vector2Int>
      {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(12, 12),
        new Vector2Int(11, 12),
        new Vector2Int(10, 12),
        new Vector2Int(11, 11),
        new Vector2Int(12, 10),
        new Vector2Int(12, 11),
        // Lamps
        new Vector2Int(9, 12),
        new Vector2Int(12, 9)
      };

      this.disabledTiles = disabled.AsReadOnly();

      for (int x = 0; x < GridSizeX; x++)
      {
        for (int y = 0; y < GridSizeY; y++)
        {
          var pos = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
          var state = Enums.TileState.FREE;

          if (this.disabledTiles.Any(v => v.Equals(new Vector2Int(x, y))))
          {
            state = Enums.TileState.DISABLED;
          }

          grid[x, y] = new Node(state, pos, x, y);
        }
      }
    }

    public Node getNode(Vector2Int position)
    {
      return this.grid[position.x, position.y];
    }

    public Node getNode(Vector3 position)
    {
      var cell = this.go_grid.WorldToCell(position);
      return grid[cell.x, cell.y];
    }

    public Node getRandomFreeNode()
    {
      var tile = getNode(new Vector2Int(Random.Range(0, 12), Random.Range(0, 12)));
      while (tile.GetState() != Enums.TileState.FREE)
      {
        tile = getNode(new Vector2Int(Random.Range(0, 12), Random.Range(0, 12)));
      }

      return tile;
    }
    
    public List<Node> GetNeighbours(Node node)
    {
      List<Node> neighbours = new List<Node>();
      
      for (int x = -1; x < 2; x++) {
        for (int y = -1; y < 2; y++) {
          var newX = node.gridX + x;
          var newY = node.gridY + y;
          // we dont want to add the current node
          if (x == 0 && y == 0) {
            continue;
          }
          // boundary check
          else if (newX >= 0 && newX < GridSizeX && newY >= 0 && newY < GridSizeY) {
            neighbours.Add(grid[newX, newY]);
          }
        }
      }

      return neighbours;
    }

    [System.Serializable]
    public class TerrainType
    {
      public LayerMask terrainMask;
      public int terrainPenalty;
    }

    /// <summary>
    /// Calculate the correct sorting order for a given position.
    /// </summary>
    /// <param name="position">Position that needs a sorting order value.</param>
    /// <param name="fine">Use fine calculation for within a grid tile.</param>
    /// <returns></returns>
    public int CalculateSortingLayer(Vector3 position, bool fine = false)
    {
      var cell = go_grid.WorldToCell(position);
      var transformedCell = new Vector2Int(13 - cell.x, 13 - cell.y);
      var baseLayer = (transformedCell.x * 12 + transformedCell.y) * SORTING_ORDER_GRANULARITY;

      if (fine)
      {
        var cellPos = getNode(new Vector2Int(cell.x, cell.y)).worldPosition;
        var posDiff = cellPos - position;
        var fineLayer = (int)((posDiff.y / (SORTING_ORDER_GRANULARITY / 2)) * 100);
        return baseLayer + fineLayer;
      }
      else
      {
        return baseLayer;
      }
    }

    /// <summary>
    /// Set the state af a tile.
    /// </summary>
    /// <param name="tile">Tile</param>
    /// <param name="state">State</param>
    public void SetNodeState(Vector2Int tile, Enums.TileState state)
    {
      grid[tile.x, tile.y].SetState(state);
    }
    
    /// <summary>
    /// Get the state of a given tile.
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public Enums.TileState GetNodeState(Vector2Int tile)
    {
      return grid[tile.x, tile.y].GetState();
    }
  }
}

