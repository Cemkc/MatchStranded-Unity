using UnityEngine;
using Flap;
using System;

public abstract class TileObject : MonoBehaviour
{
    public abstract void Init();

    protected Tile _parentTile;
    protected TileObjectType _type;
    protected TileObjectCategory _category;
    protected bool _clickable;

    public Tile ParentTile{ get => _parentTile; set => _parentTile = value; }
    public TileObjectType Type{ get => _type; }
    public TileObjectCategory Category{ get => _category; }
    public bool Clickable{ get => _clickable; }

    void Awake()
    {
        Init();
    }
}

public enum TileObjectType
{
    Absent = 0,
    None,
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Balloon
}

[Flags]
public enum TileObjectCategory
{
    Absent = 0,
    HittableTileObject = 1 << 0,
    ConstantTileObject = 1 << 1,
    ClickableTileObject = 1 << 2,
    FallableTileObject = 1 << 3
}