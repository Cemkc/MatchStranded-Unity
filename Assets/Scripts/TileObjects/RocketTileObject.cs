using System.Collections;
using Flap;
using UnityEngine;

public class RocketTileObject : ClickableTileObject, IHitableTileobject
{
    private bool _isVertical;

    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Rocket;
        _category |= TileObjectCategory.HitableTileObject;
        _isVertical = Random.value > 0.5f;
    }

    public override void OnClick()
    {
        StartCoroutine(FireRocket(_parentTile.TileId));
        // OnDestroy?.Invoke(_parentTile.TileId);
    }

    public void OnHit(int damage)
    {
        // StartCoroutine(FireRocket(_parentTile.TileId));
        // OnDestroy?.Invoke(_parentTile.TileId);
    }

    IEnumerator FireRocket(int tileNum)
    {
        LevelManager.s_Instance.RunningSequences++;

        int nextTileDelta = _isVertical ? 1 : LevelManager.GridDimension;

        Vector2Int tileAPos = LevelManager.TileIdToPos(tileNum);
        Vector2Int tileBPos = LevelManager.TileIdToPos(tileNum);

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

            Debug.Log("Rocket was on tile: " + tileNum + " Verticality is: " + _isVertical + " Tile A is: " + tileAPos + " Tile B is: " + tileBPos);
            
            Debug.Log("Calling get with " + tileAPos);
            Tile tileA = LevelManager.s_Instance.GetTile(tileAPos);
            if(tileA != null && tileA.GetTileCategory().HasFlag(TileObjectCategory.HitableTileObject))
            {
                IHitableTileobject hitableTileobject = tileA.ActiveTileObject() as IHitableTileobject;
                hitableTileobject.OnHit(1);
            }
            
            Debug.Log("Calling get with " + tileBPos);
            Tile tileB = LevelManager.s_Instance.GetTile(tileBPos);
            if(tileB != null && tileB.GetTileCategory().HasFlag(TileObjectCategory.HitableTileObject))
            {
                IHitableTileobject hitableTileobject = tileB.ActiveTileObject() as IHitableTileobject;
                hitableTileobject.OnHit(1);
            }

            if(tileA == null && tileB == null){
                Debug.Log("Breaking!!!");
                break;
            }

            yield return new WaitForSeconds(0.05f);
        }

        OnDestroy(tileNum);

        LevelManager.s_Instance.RunningSequences--;

    }
}