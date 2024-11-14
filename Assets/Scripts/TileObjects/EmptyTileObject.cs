
public class EmptyTileObject : TileObject
{
    public override void OnEnableFunction()
    {
        _type = TileObjectType.None;
        _category = TileObjectCategory.None;
    }
}
