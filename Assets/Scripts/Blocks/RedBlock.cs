using UnityEngine;

public class RedBlock : Block
{
    private TileObjType _blockType;

    public RedBlock()
    {
        _blockType = TileObjType.Red;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
