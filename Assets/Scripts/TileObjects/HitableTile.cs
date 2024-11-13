using Flap;
using UnityEngine.Events;

public class HitableTile : TileObject
{
    protected UnityAction<Tile> OnBreak;

    public override void Init()
    {
    }

    public virtual void OnHit(int damage){}
}
