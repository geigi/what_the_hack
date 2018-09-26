using UnityEngine;
using System.Collections;
using static Utils.Enums;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class represents a tile used for the tilemap. The StatefulTile always has a state (free, occupied, ...)
/// and sometimes an occupying object.
/// </summary>
[CreateAssetMenu(menuName = "What_The_Hack/Sateful Tile")]
public class StatefulTile : TileBase
{
    public Sprite sprite;
    public Color color;
    TileState state = TileState.FREE;
    GameObject occupying_object = null;

    /// <summary>
    /// Sets the state of this tile.
    /// </summary>
    /// <param name="state">State.</param>
    /// <param name="obj">Object occuping this state. Pass null if state is FREE.</param>
    public void setState(TileState state, GameObject obj) {
        this.state = state;
        this.occupying_object = obj;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <returns>The state.</returns>
    public TileState getState() {
        return this.state;
    }

    /// <summary>
    /// Gets the occupying object.
    /// </summary>
    /// <returns>The occupying object.</returns>
    public GameObject getOccupyingObject() {
        return this.occupying_object;
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData) {
        if (this.state != TileState.DISABLED)
            tileData.color = this.color;
        else
            tileData.color = new Color(230, 230, 230, 0.6f);
        
        tileData.sprite = this.sprite;
        tileData.colliderType = ColliderType.None;
    }
}
