using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Pathfinding
{
  public class AGrid : MonoBehaviour
  {
    Node[,] grid;

    private GridInformation gridInformation;

    int gridSizeX = 13, gridSizeY = 13;

    private IReadOnlyList<Vector2Int> disabledTiles;

    void Awake()
    {
      CreateGrid();
    }

    public int MaxSize
    {
      get { return gridSizeX * gridSizeY; }
    }

    void CreateGrid()
    {
      grid = new Node[gridSizeX, gridSizeY];

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
        new Vector2Int(12, 11)
      };

      this.disabledTiles = disabled.AsReadOnly();

      for (int x = 0; x < gridSizeX; x++)
      {
        for (int y = 0; y < gridSizeY; y++)
        {
          var pos = new Vector2Int(x, y);
          var state = Enums.TileState.FREE;

          if (this.disabledTiles.Any(v => v.Equals(pos)))
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
    
    public List<Node> GetNeighbours(Node node)
    {
      List<Node> neighbours = new List<Node>();
      List<Tuple<int, int>> toTest = new List<Tuple<int, int>>
      {
        new Tuple<int, int>(node.gridX - 1, node.gridY),
        new Tuple<int, int>(node.gridX + 1, node.gridY),
        new Tuple<int, int>(node.gridX, node.gridY - 1),
        new Tuple<int, int>(node.gridX, node.gridY + 1)
      };
      
      foreach (var p in toTest)
      {
        if (p.Item1 >= 0 && p.Item1 < gridSizeX && p.Item2 >= 0 && p.Item2 < gridSizeY)
          {
            neighbours.Add(grid[p.Item1, p.Item2]);
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


  }
}

