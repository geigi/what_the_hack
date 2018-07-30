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
      while (tile.state != Enums.TileState.FREE)
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
          else if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY) {
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


  }
}

