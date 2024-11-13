public class RedBlock : Block
{
    public override void Init()
    {
        _blockType = TileObjType.Red;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
