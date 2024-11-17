using UnityEngine;
using Flap;

public class AbsentTile : Tile
{
    public override TileObject ActiveTileObject()
    {
        return null;
    }

    public override void DestroyTileObject(){}

    public override TileObjectCategory GetTileCategory()
    {
        return TileObjectCategory.Absent;
    }

    public override TileObjectType GetTileType()
    {
        return TileObjectType.Absent;
    }

    public override void Init(int col, int row)
    {
        _tilePos = new Vector2Int(col, row);
    }

    public override void PlayParticle(ParticleName particleName)
    {
        throw new System.NotImplementedException();
    }

    public override void SetTileObject(TileObject tileObject)
    {
        Debug.Log("Setting an absent tile is not allowed.");
    }

    public override void SetTileObject(TileObjectType type)
    {
        Debug.Log("Setting an absent tile is not allowed.");
    }

}