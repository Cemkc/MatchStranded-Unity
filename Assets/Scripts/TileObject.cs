using UnityEngine;

public enum TileObjType
{
    None,
    Red,
    Blue,
    Green,
    Yellow,
    Purple
}

public abstract class TileObject : MonoBehaviour
{
    public abstract TileObjType GetTileObjType();
}