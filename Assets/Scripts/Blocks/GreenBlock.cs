
public class GreenBlock : Block
{
    public override void Init()
    {
        _blockType = TileObjType.Green;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
