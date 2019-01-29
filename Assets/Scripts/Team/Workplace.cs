using Pathfinding;
using UnityEditor.UI;
using UnityEngine;
using Utils;

namespace Team
{
    /// <summary>
    /// This component represents a workplace.
    /// It is responsible for blocking the grid and displaying the sprites.
    /// It is also responsible for managing the progress of a mission.
    /// </summary>
    public class Workplace : MonoBehaviour
    {
        public AGrid Grid;
        public bool EnableOnStart = false;
        public SpriteRenderer Desk;
        public SpriteRenderer Pc;
        public SpriteRenderer Chair;

        private Vector2Int Position;

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

        /// <summary>
        /// Enable this workplace.
        /// </summary>
        /// <param name="enable">Enable?</param>
        public void Enable(bool enable)
        {
            if (enable)
            {
                gameObject.SetActive(true);
                Grid.SetNodeState(Position, Enums.TileState.BLOCKED);
                Grid.SetNodeState(new Vector2Int(Position.x, Position.y - 1), Enums.TileState.BLOCKED);
            }
            else
            {
                gameObject.SetActive(false);
                Grid.SetNodeState(Position, Enums.TileState.FREE);
            }
        }
    }
}