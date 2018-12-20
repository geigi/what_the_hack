using SaveGame;

namespace Interfaces
{
    /// <summary>
    /// This interface should be used by all classes which need data to be saved
    /// in a savegame.
    /// The classes must handle deserialization on their own in the Start method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Saveable<T>
    {
        /// <summary>
        /// Return the serializable object that will be saved in a savegame.
        /// </summary>
        /// <returns>Serializable object</returns>
        T GetData();
    }
}