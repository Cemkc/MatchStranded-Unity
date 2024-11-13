using UnityEngine;

public enum TileObjType
{
    None,
    Absent,
    Red,
    Blue,
    Green,
    Yellow,
    Purple
}

public abstract class TileObject : MonoBehaviour
{
    public abstract TileObjType GetTileObjType();
    public abstract void Init();

    void Awake()
    {
        Init();
    }
}