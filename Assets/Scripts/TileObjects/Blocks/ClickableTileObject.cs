
public abstract class ClickableTileObject : TileObject
{
    public abstract void OnClick();

    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _category = _category = TileObjectCategory.ClickableTileObject;
    }
    
}
