using Employees;
using SaveGame;

namespace Interfaces
{
    /// <summary>
    /// This interface is used for managers like the GameTime or the <see cref="EmployeeManager"/>.
    /// It ensures correct initialization.
    /// </summary>
    public interface Manager
    {
        /// <summary>
        /// Init references to other managers.
        /// </summary>
        void InitReferences();

        /// <summary>
        /// Set the manager to a default state (new game).
        /// </summary>
        void InitDefaultState();

        /// <summary>
        /// Load state from a savegame.
        /// </summary>
        /// <param name="mainSaveGame"></param>
        void LoadState(MainSaveGame mainSaveGame);

        /// <summary>
        /// Clean all data.
        /// </summary>
        void Cleanup();
    }
}