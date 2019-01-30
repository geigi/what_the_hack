using System.Collections.Generic;
using Interfaces;
using Pathfinding;
using UnityEngine;
using Utils;

namespace Team
{
    /// <summary>
    /// This component represents a workplace.
    /// It is responsible for blocking the grid and displaying the sprites.
    /// It is also responsible for managing the progress of a mission.
    /// </summary>
    public class Workplace : MonoBehaviour, Touchable
    {
        public AGrid Grid;
        public bool EnableOnStart = false;
        public SpriteRenderer Desk;
        public SpriteRenderer Pc;
        public SpriteRenderer Chair;

        private Vector2Int Position;
        private bool testTiles = false;
        private Vector2Int position2;

        private void Start()
        {
            var pos = transform.position + new Vector3(-0.2f, -0.2f, 0);
            var cellPos = Grid.go_grid.WorldToCell(pos);
            Position = new Vector2Int(cellPos.x, cellPos.y);
            Debug.Log(Position);
            var layer = Grid.CalculateSortingLayer(pos);
            Desk.sortingOrder = layer;
            Pc.sortingOrder = layer + 1;
            Chair.sortingOrder = layer - 1;
            Enable(EnableOnStart);
        }

        private void Update()
        {
            if (testTiles)
            {
                if (Grid.GetNodeState(Position) == Enums.TileState.FREE &&
                    Grid.GetNodeState(position2) == Enums.TileState.FREE)
                {
                    testTiles = false;
                    SetActive(true);
                }
            }
        }

        /// <summary>
        /// Enable this workplace.
        /// </summary>
        /// <param name="enable">Enable?</param>
        public void Enable(bool enable)
        {
            if (enable)
            {
                // A Workplace blocks 2 tiles
                position2 = new Vector2Int(Position.x, Position.y - 1);
                
                // Test if tiles are blocked/occupied
                var node1State = Grid.GetNodeState(Position);
                var node2State = Grid.GetNodeState(position2);
                if (node1State != Enums.TileState.FREE || node2State != Enums.TileState.FREE)
                {
                    // If they are occupied we test each frame if the tile is now free
                    gameObject.SetActive(true);
                    testTiles = true;
                }
                else
                {
                    SetActive(true);
                }
            }
            else
            {
                SetActive(false);
            }
        }

        private void SetActive(bool active)
        {
            if (active)
            {
                gameObject.SetActive(true);
                Desk.enabled = true;
                Chair.enabled = true;
                Pc.enabled = true;
                Grid.SetNodeState(Position, Enums.TileState.BLOCKED);
                Grid.SetNodeState(position2, Enums.TileState.BLOCKED);
            }
            else
            {
                gameObject.SetActive(false);
                Desk.enabled = false;
                Chair.enabled = false;
                Pc.enabled = false;
                Grid.SetNodeState(Position, Enums.TileState.FREE);
                Grid.SetNodeState(position2, Enums.TileState.FREE);
            }
        }

        public void Touched()
        {
            throw new System.NotImplementedException();
        }
    }
}