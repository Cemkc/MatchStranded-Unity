using UnityEngine;

public class BalloonTileObject : TileObject, IHitableTileobject, IMatchSensitive, IAudible
{
    private int _health;

    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _health = 1;
        _type = TileObjectType.Balloon;
        _category = TileObjectCategory.HitableTileObject | TileObjectCategory.FallableTileObject
                    | TileObjectCategory.MatchSensitiveObject | TileObjectCategory.AudibleTileObject;
    }

    public void OnHit(int damage)
    {
        _health -= damage;

        if(_health <= 0){
            OnDestroy?.Invoke(_parentTile, this);
        }
    }

    public void OnMatchHit()
    {
        OnHit(1);
    }

    public AudioName GetAudioName()
    {
        return AudioName.Balloon;
    }
}