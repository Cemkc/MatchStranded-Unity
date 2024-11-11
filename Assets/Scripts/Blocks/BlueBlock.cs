using UnityEngine;

public class BlueBlock : Block
{
    private TileObjType _blockType;

    public BlueBlock()
    {
        _blockType = TileObjType.Blue;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
