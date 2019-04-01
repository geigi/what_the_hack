namespace GameSystem
{
    /// <summary>
    /// This class stores non permanent game settings across scenes.
    /// It is used to pass data from one scene to another.
    /// </summary>
    public static class GameSettings
    {
        /// <summary>
        /// Is the game a new game?
        /// </summary>
        public static bool NewGame = true;
        
        /// <summary>
        /// Name of the savegame to be loaded.
        /// Is null when y new game is selected.
        /// </summary>
        public static string SaveGameName;
        
        /// <summary>
        /// It of the mod that should be loaded.
        /// Can be null.
        /// </summary>
        public static string ModID;

        public static MissionList.DifficultyOption Difficulty = MissionList.DifficultyOption.Easy;
    }
}