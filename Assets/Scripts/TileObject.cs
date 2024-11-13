using UnityEngine;
using Flap;

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
    public abstract void Init();

    protected Tile _parentTile;
    protected TileObjType _type;
    protected bool _clickable;

    public Tile ParentTile{ get => _parentTile; set => _parentTile = value; }
    public TileObjType Type{ get => _type; }
    public bool Clickable{ get => _clickable; }

    void Awake()
    {
        Init();
    }
}