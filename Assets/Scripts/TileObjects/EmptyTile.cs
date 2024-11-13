
public class EmptyTile : TileObject
{
    public override void Init()
    {
        _clickable = false;
        _type = TileObjType.None;
    }
}
