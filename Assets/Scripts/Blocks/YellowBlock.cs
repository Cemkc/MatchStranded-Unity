
public class YellowBlock : Block
{
    public override void Init()
    {
        _blockType = TileObjType.Yellow;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }

}
