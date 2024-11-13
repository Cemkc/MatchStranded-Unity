
public class BlueBlock : Block
{
    public override void Init()
    {
        _blockType = TileObjType.Blue;
    }

    public override TileObjType GetTileObjType()
    {
        return _blockType;
    }
}
