using System.Collections;
using Flap;
using UnityEngine;

public class RocketTileObject : ClickableTileObject, IHitableTileobject
{
    private bool _isVertical;
    private bool _rocketFired;

    [SerializeField] private Sprite _verticalRocketSprite;
    [SerializeField] private Sprite _horizontalRocketSprite;

    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Rocket;
        _category |= TileObjectCategory.HitableTileObject;
    }

    public override void OnEnableFunction()
    {
        base.OnEnableFunction();
        _isVertical = Random.value > 0.5f;
        _rocketFired = false;

        
        SpriteRenderer spriteRenderer;
        if(transform.TryGetComponent(out spriteRenderer))
        {
            if(_isVertical) spriteRenderer.sprite = _verticalRocketSprite;
            else spriteRenderer.sprite = _horizontalRocketSprite;
        }
    }

    public override bool OnClick()
    {
        StartCoroutine(FireRocket(_parentTile.TileId));
        return true;
    }

    public void OnHit(int damage)
    {
        if(!_rocketFired) StartCoroutine(FireRocket(_parentTile.TileId));
    }

    IEnumerator FireRocket(int tileNum)
    {
        GridManager.s_Instance.RunningSequences++;

        _rocketFired = true;

        int nextTileDelta = _isVertical ? 1 : GridManager.GridDimension;

        Vector2Int tileAPos = GridUtils.TileIdToPos(tileNum);
        Vector2Int tileBPos = GridUtils.TileIdToPos(tileNum);

        while(true)
        {
            if(_isVertical){
                tileAPos.y += 1;
                tileBPos.y -= 1;
            }
            else{
                tileAPos.x += 1;
                tileBPos.x -= 1;
            }

            Tile tileA = GridManager.s_Instance.GetTile(tileAPos);
            if(tileA != null && tileA.GetTileCategory().HasFlag(TileObjectCategory.HitableTileObject))
            {
                IHitableTileobject hitableTileobject = tileA.ActiveTileObject() as IHitableTileobject;
                hitableTileobject.OnHit(1);
            }
            
            Tile tileB = GridManager.s_Instance.GetTile(tileBPos);
            if(tileB != null && tileB.GetTileCategory().HasFlag(TileObjectCategory.HitableTileObject))
            {
                IHitableTileobject hitableTileobject = tileB.ActiveTileObject() as IHitableTileobject;
                hitableTileobject.OnHit(1);
            }

            if(tileA == null && tileB == null){
                break;
            }

            yield return new WaitForSeconds(0.05f);
        }

        OnDestroy(_parentTile, this);

        GridManager.s_Instance.RunningSequences--;

    }
}