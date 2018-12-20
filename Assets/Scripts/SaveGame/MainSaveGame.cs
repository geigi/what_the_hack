using System;
using System.Collections.Generic;
using GameTime;
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
		/// Employees that are available for hire.
		/// </summary>
		public List<EmployeeData> employeesForHire { get; set; }
		
		/// <summary>
		/// Employees that are hired.
		/// </summary>
		public List<EmployeeData> employeesHired { get; set; }
		
		/// <summary>
		/// Employees that were previously hired.
		/// </summary>
		public List<EmployeeData> exEmployees { get; set; }
	}
}
