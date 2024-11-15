using UnityEngine;
using Flap;
using System;
using UnityEngine.Events;

public abstract class TileObject : MonoBehaviour
{
    protected UnityAction<Tile, TileObject> OnDestroy;

    protected Tile _parentTile;
    protected TileObjectType _type;
    protected TileObjectCategory _category;

    public Tile ParentTile{ get => _parentTile; set => _parentTile = value; }
    public TileObjectType Type{ get => _type; }
    public TileObjectCategory Category{ get => _category; }

    public virtual void OnAwakeFunction(){}

    public virtual void OnEnableFunction()
    {
        OnDestroy += GridManager.s_Instance.OnTileDestroy;
    }

    public virtual void OnDisableFunction()
    {
        OnDestroy -= GridManager.s_Instance.OnTileDestroy;
    }

    #region MonoBehaviour Functions
    void Awake()
    {
        OnAwakeFunction();
    }

    void OnEnable()
    {
        OnEnableFunction();
    }

    void OnDisable()
    {
        OnDisableFunction();
    }
    #endregion
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
    Balloon,
    Rocket,
    Duck
}

[Flags]
public enum TileObjectCategory
{
    Absent = 0,
    None = 1 << 0,
    HitableTileObject = 1 << 1,
    ConstantTileObject = 1 << 2,
    ClickableTileObject = 1 << 3,
    FallableTileObject = 1 << 4,
    MatchSensitiveObject = 1 << 5,
    AudibleTileObject = 1 << 6,
    ParticleEmittingTileobject = 1 << 7
}