using System.Collections.Generic;
using System.Linq;
using GameSystem;
using Interfaces;
using Missions;
using Pathfinding;
using UnityEngine;
using Utils;
using Wth.ModApi.Employees;

namespace Team
{
    /// <summary>
    /// This component represents a workplace.
    /// It is responsible for blocking the grid and displaying the sprites.
    /// It is also responsible for managing the progress of a mission.
    /// </summary>
    public class Workplace : MonoBehaviour, Touchable, Saveable<WorkplaceData>
    {
        public AGrid Grid;
        public bool EnableOnStart = false;
        public SpriteRenderer Desk;
        public SpriteRenderer Pc;
        public SpriteRenderer Chair;
        public Vector2Int Position;
        public Animator Animator;
        
        private static readonly int idleProperty = Animator.StringToHash("idle");
        private static readonly int workingProperty = Animator.StringToHash("working");
        
        private bool testTiles = false;
        private Vector2Int position2;
        private Vector2Int position3;

        private WorkplaceData data;

        private Employee employee;

        private void Awake()
        {
            if  (GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();
        }
        
        private void LoadState()
        {
            var mainSaveGame = ContentHub.Instance.SaveGameSystem.GetCurrentSaveGame();
            data = mainSaveGame.WorkplaceDatas.First(d => d.Position.Equals(Position));
        }

        private void InitDefaultState()
        {
            data = new WorkplaceData();
            data.Position = Position;
        }
        
        private void Start()
        {
            var BottomTilePosition = Grid.go_grid.CellToWorld(new Vector3Int(Position.x, Position.y + 1, 0));
            var layer = Grid.CalculateSortingLayer(BottomTilePosition);
            Desk.sortingOrder = layer;
            Pc.sortingOrder = layer + 1;
            Chair.sortingOrder = layer - 2;
            Enable(EnableOnStart);
        }

        private void Update()
        {
            if (testTiles)
            {
                if (Grid.GetNodeState(Position) == Enums.TileState.FREE &&
                    Grid.GetNodeState(position2) == Enums.TileState.FREE && 
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
                position3 = new Vector2Int(Position.x, Position.y + 1);
                
                // Test if tiles are blocked/occupied
                var node1State = Grid.GetNodeState(Position);
                var node2State = Grid.GetNodeState(position2);
                var node3State = Grid.GetNodeState(position3);
                if (node1State != Enums.TileState.FREE || node2State != Enums.TileState.FREE || node3State != Enums.TileState.FREE)
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

        /// <summary>
        /// Get the correct sorting order for an occupying employee.
        /// </summary>
        /// <returns></returns>
        public int GetEmployeeSortingOrder()
        {
            return Chair.sortingOrder + 1;
        }

        /// <summary>
        /// Set this workplace active/inactive.
        /// Enables rendering and blocks tiles.
        /// </summary>
        /// <param name="active"></param>
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
                Grid.SetNodeState(position3, Enums.TileState.BLOCKED);
            }
            else
            {
                gameObject.SetActive(false);
                Desk.enabled = false;
                Chair.enabled = false;
                Pc.enabled = false;
                Grid.SetNodeState(Position, Enums.TileState.FREE);
                Grid.SetNodeState(position2, Enums.TileState.FREE);
                Grid.SetNodeState(position3, Enums.TileState.FREE);
            }
        }

        public void Occupy(Employee employee, Mission mission)
        {
            data.Mission = mission;
            this.employee = employee;
        }

        /// <summary>
        /// Start working on this workplace.
        /// </summary>
        public void StartWorking()
        {
            Animator.SetTrigger(workingProperty);
        }

        /// <summary>
        /// Stop any work on this workplace.
        /// </summary>
        public void StopWorking()
        {
            Animator.SetTrigger(idleProperty);
            employee.StopWorking();
            employee = null;
            data.Mission = null;
        } 

        /// <summary>
        /// Is this workplace currently occupied?
        /// </summary>
        /// <returns></returns>
        public bool IsOccupied()
        {
            if (employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets called when the gameobject is touched or clicked.
        /// </summary>
        public void Touched()
        {
            
        }

        public WorkplaceData GetData()
        {
            if (employee != null)
            {
                data.OccupyingEmployee = employee.EmployeeData;
            }
            return data;
        }

        /// <summary>
        /// Get the employee that currently occupies this workplace.
        /// </summary>
        /// <returns></returns>
        public Employee GetOccupyingEmployee()
        {
            return employee;
        }
    }
}