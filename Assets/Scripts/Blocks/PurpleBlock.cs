
public class PurpleBlock : Block
{
    public override void Init()
    {
        _blockType = TileObjType.Purple;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
