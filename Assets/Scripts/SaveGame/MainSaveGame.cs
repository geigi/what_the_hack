﻿using System;
using System.Collections.Generic;
using Employees;
using GameTime;
using Missions;
using Team;
using Wth.ModApi.Employees;

namespace SaveGame
{
	/// <summary>
	/// This class represents a savegame. It contains all data that needs to be persistent
	/// for a game to be continued.
	/// </summary>
	[Serializable]
	public class MainSaveGame
	{
		/// <summary>
		/// Name of the savegame.
		/// </summary>
		public string name { get; set; }
		
		/// <summary>
		/// ID of the mod that was used with this savegame.
		/// </summary>
		public string modId { get; set; }
		
		/// <summary>
		/// Date when this savegame was created.
		/// </summary>
		public DateTime saveDate { get; set; }
	
		/// <summary>
		/// Data of the tilemap.
		/// TODO: Maybe this is not necessary.
		/// </summary>
		public NodeData[,] Tilemap { get; set; }
		
		/// <summary>
		/// The game time.
		/// </summary>
		public GameTimeData gameTime { get; set; }

		/// <summary>
		/// All persistent data from employee manager.
		/// </summary>
		public EmployeeManagerData employeeManagerData;
		
		/// <summary>
		/// All persistent data from employee manager.
		/// </summary>
		public MissionManagerData missionManagerData;

		/// <summary>
		/// All persistent data from team manager.
		/// </summary>
		public TeamManagerData teamManagerData;

		public List<WorkplaceData> WorkplaceDatas;
		
        /// <summary>
        /// The current balance on this savegame.
        /// </summary>
        public int balance;
    }
}
