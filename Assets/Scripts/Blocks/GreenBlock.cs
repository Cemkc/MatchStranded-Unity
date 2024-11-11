
public class GreenBlock : Block
{
    private TileObjType _blockType;

    public GreenBlock()
    {
        _blockType = TileObjType.Green;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
