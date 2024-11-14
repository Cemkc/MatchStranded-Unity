using UnityEngine;
using System.Collections.Generic;
using Flap;

public abstract class Block : ClickableTileObject, IHitableTileobject
{
    protected int _health;

    public override void OnAwakeFunction(){
        base.OnAwakeFunction();
        _category |= TileObjectCategory.HitableTileObject;
    }

    public override void OnEnableFunction()
    {
        base.OnEnableFunction();
        _health = 1;
    }

    public override void OnClick()
    {
        List<int> connectedTiles = new List<int>();
        List<int> hitTiles = new List<int>();
        int tileNumber = GridManager.TilePosToId(_parentTile.TilePos);
        GridManager.GetConnectedTiles(tileNumber, ref connectedTiles, ref hitTiles);

        if(connectedTiles != null && connectedTiles.Count > 1)
        {
            foreach(int tileNum in connectedTiles)
            {
                GridManager.s_Instance.OnTileDestroy(tileNum);
                // OnDestroy?.Invoke(tileNum);
            }

            foreach (int tileNum in hitTiles)
            {
                Tile tile = GridManager.s_Instance.GetTile(tileNum);
                if(!tile.GetTileCategory().HasFlag(TileObjectCategory.MatchSensitiveObject)) continue; // We put same tiles into the list to be able to hit them multiple times but if the tile is gone-broke that means we should not do a cast
                IMatchSensitive matchSensitiveTile = tile.ActiveTileObject() as IMatchSensitive;
                matchSensitiveTile.OnMatchHit();
            }

            if(connectedTiles.Count >= 5)
            {
                Debug.Log("Trying to set tile pos: " + _parentTile.TilePos + " To rocket");
                _parentTile.SetTile(TileObjectType.Rocket);
            }

        }
    }

    public void OnHit(int damage)
    {
        _health -= damage;

        if(_health <= 0){
            // Init(); // To reset the variables (not sure if it is a good way to do that)
            OnDestroy?.Invoke(_parentTile.TileId);
        }
    }
}