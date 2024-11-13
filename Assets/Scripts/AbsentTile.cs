using UnityEngine;
using Flap;

public class AbsentTile : Tile
{
    public override TileObjType GetTileObjType()
    {
        return TileObjType.Absent;
    }

    public override void Init(int col, int row)
    {
        _tilePos = new Vector2Int(col, row);
    }

    public override void OnHit()
    {
    }

    public override void SetTile(TileObjType type)
    {
        Debug.Log("Setting an absent tile is not allowed.");
        return;
    }
}