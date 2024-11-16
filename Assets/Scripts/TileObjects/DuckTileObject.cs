using UnityEngine;

public class DuckTileObject : TileObject, IAudible
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Duck;
        _category |= TileObjectCategory.FallableTileObject | TileObjectCategory.AudibleTileObject;
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
        if(_parentTile != null && _parentTile.TilePos.y == 0)
        {
            OnDestroy?.Invoke(_parentTile, this);
        }
    }

    public AudioName GetAudioName()
    {
        return AudioName.Duck;
    }
}
