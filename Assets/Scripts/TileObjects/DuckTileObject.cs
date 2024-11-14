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
        LevelManager.s_Instance.OnFillEnd += FillEndCallback;
    }

    public override void OnDisableFunction()
    {
        LevelManager.s_Instance.OnFillEnd -= FillEndCallback;
    }

    private void FillEndCallback()
    {
        Debug.Log("Heard fill end, I'm on tile: " + _parentTile.TilePos);
        if(_parentTile.TilePos.y == 0)
        {
            OnDestroy?.Invoke(_parentTile.TileId);
        }
    }

}
