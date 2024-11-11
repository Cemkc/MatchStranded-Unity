using UnityEngine;

public enum TileObjType
{
    None,
    Red,
    Blue,
    Green
}

public abstract class TileObject : MonoBehaviour
{
    public abstract TileObjType GetTileObjType();
}