using UnityEngine;

public class DuckTileObject : TileObject
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Duck;
        _category |= TileObjectCategory.FallableTileObject;
    }

    public override void OnEnableFunction()
    {
        base.OnEnableFunction();
        GridManager.s_Instance.OnFillEnd += FillEndCallback;
    }

    public override void OnDisableFunction()
    {
        GridManager.s_Instance.OnFillEnd -= FillEndCallback;
    }

    private void FillEndCallback()
    {
        if(_parentTile.TilePos.y == 0)
        {
            OnDestroy?.Invoke(_parentTile.TileId);
        }
    }

}
