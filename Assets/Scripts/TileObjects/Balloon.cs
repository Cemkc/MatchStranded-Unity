public class Balloon : HitableTile
{
    private int _health;

    public override void Init()
    {
        base.Init();
        OnBreak += LevelManager.s_Instance.OnTileBroke;
        _type = TileObjectType.Balloon;
        _category = TileObjectCategory.HittableTileObject | TileObjectCategory.FallableTileObject;
    }

    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        _health -= damage;

        if(_health <= 0){
            OnBreak?.Invoke(ParentTile);
        }
    }

}