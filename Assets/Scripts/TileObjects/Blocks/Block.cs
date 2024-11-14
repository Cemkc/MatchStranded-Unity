using UnityEngine;
using System.Collections.Generic;
using Flap;

public abstract class Block : ClickableTileObject, IHitableTileobject
{
    protected int _health;

    public override void OnAwakeFunction(){
        base.OnAwakeFunction();
        Debug.Log("Trying to create block object. ");
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
        int tileNumber = LevelManager.TilePosToId(_parentTile.TilePos);
        LevelManager.GetConnectedTiles(tileNumber, ref connectedTiles, ref hitTiles);
        string str = "Cliked at tile: " + tileNumber + ", connected tiles are: ";
        foreach (int tile in connectedTiles)
        {
            str += tile + ", ";
        }
        Debug.Log(str);

        string strHittable = "Cliked at tile: " + tileNumber + ", hittable tiles are: ";
        foreach (int tile in hitTiles)
        {
            strHittable += tile + ", ";
        }
        Debug.Log(strHittable);

        if(connectedTiles != null && connectedTiles.Count > 1)
        {
            foreach(int tileNum in connectedTiles)
            {
                Debug.Log("Trying to destroy tile: " + tileNum);
                LevelManager.s_Instance.OnTileDestroy(tileNum);
                // OnDestroy?.Invoke(tileNum);
            }

            foreach (int tileNum in hitTiles)
            {
                Tile tile = LevelManager.s_Instance.GetTile(tileNum);
                if(!tile.GetTileCategory().HasFlag(TileObjectCategory.MatchSensitiveObject)) continue; // We put same tiles into the list to be able to hit them multiple times but if the tile is gone-broke that means we should not do a cast
                IMatchSensitive matchSensitiveTile = tile.ActiveTileObject() as IMatchSensitive;
                Debug.Log("Trying to match hit tile: " + tileNum);
                matchSensitiveTile.OnMatchHit();
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