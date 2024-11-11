using UnityEngine;

public class YellowBlock : Block
{
    private TileObjType _blockType;

    public YellowBlock()
    {
        _blockType = TileObjType.Yellow;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
