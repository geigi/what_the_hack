using UnityEngine;
using Utils;

public class TileInformation
{
    Enums.TileState state = Enums.TileState.FREE;
    GameObject occupying_object = null;

    /// <summary>
    /// Sets the state of this tile.
    /// </summary>
    /// <param name="state">State.</param>
    /// <param name="obj">Object occuping this state. Pass null if state is FREE.</param>
    public void setState(Enums.TileState state, GameObject obj)
    {
        this.state = state;
        this.occupying_object = obj;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <returns>The state.</returns>
    public Enums.TileState getState()
    {
        return this.state;
    }

    /// <summary>
    /// Gets the occupying object.
    /// </summary>
    /// <returns>The occupying object.</returns>
    public GameObject getOccupyingObject()
    {
        return this.occupying_object;
    }
}