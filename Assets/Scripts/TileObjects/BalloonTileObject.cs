using UnityEngine;

public class BalloonTileObject : TileObject, IHitableTileobject, IMatchSensitive
{
    private int _health;

    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _health = 1;
        _type = TileObjectType.Balloon;
        _category = TileObjectCategory.HitableTileObject | TileObjectCategory.FallableTileObject | TileObjectCategory.MatchSensitiveObject;
    }

    public void OnHit(int damage)
    {
        Debug.Log("Health: " + _health + ", Damage: " + damage);
        _health -= damage;

        if(_health <= 0){
            OnDestroy?.Invoke(_parentTile.TileId);
        }
    }

    public void OnMatchHit()
    {
        OnHit(1);
    }
}