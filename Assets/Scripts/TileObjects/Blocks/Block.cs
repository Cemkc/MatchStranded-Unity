using UnityEngine;
using System.Collections.Generic;
using Flap;

public abstract class Block : ClickableTileObject, IHitableTileobject, IAudible, IParticleEmitting
{
    protected int _health;

    public override void OnAwakeFunction(){
        base.OnAwakeFunction();
        _category |= TileObjectCategory.HitableTileObject | TileObjectCategory.AudibleTileObject | TileObjectCategory.ParticleEmittingTileobject;
    }

    public override void OnEnableFunction()
    {
        base.OnEnableFunction();
        _health = 1;
    }

    public override bool OnClick()
    {
        List<int> connectedTiles = new List<int>();
        List<int> hitTiles = new List<int>();
        int tileNumber = _parentTile.TileId;
        GridUtils.GetConnectedTiles(tileNumber, ref connectedTiles, ref hitTiles);

        if(connectedTiles != null && connectedTiles.Count > 1)
        {
            foreach(int tileNum in connectedTiles)
            {
                Tile tile = GridManager.s_Instance.GetTile(tileNum); // Not that great of a way to to this too many back and forth commuincation and dependency
                TileObject tileObject = tile.ActiveTileObject();
                // GridManager.s_Instance.OnTileDestroy(tile, tile.ActiveTileObject());
                tileObject.OnDestroy?.Invoke(tile, tileObject);
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
                GridManager.s_Instance.GetTile(tileNumber).SetTileObject(TileObjectType.Rocket);
            }

            return true;
        }

        return false;
    }

    public void OnHit(int damage)
    {
        _health -= damage;

        if(_health <= 0){
            // Init(); // To reset the variables (not sure if it is a good way to do that)
            OnDestroy?.Invoke(_parentTile, this);
        }
    }

    public AudioName GetAudioName()
    {
        return AudioName.CubeExplode;
    }

    public ParticleName GetParticleName()
    {
        return ParticleName.CubeExplode;
    }

    public abstract Color GetParticleColor();
}