using UnityEngine;

public class PurpleBlock : Block
{
    private TileObjType _blockType;

    public PurpleBlock()
    {
        _blockType = TileObjType.Purple;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
