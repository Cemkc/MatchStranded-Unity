using UnityEngine;
using Flap;

public class AbsentTile : Tile
{
    public override TileObject ActiveTileObject()
    {
        throw new System.NotImplementedException();
    }

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

    public override void OnHit()
    {
    }

    public override void SetTile(TileObjectType type)
    {
        Debug.Log("Setting an absent tile is not allowed.");
        return;
    }
}