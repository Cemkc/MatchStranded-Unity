using UnityEngine;
using Flap;
using System.Collections.Generic;

public class PresentTile : Tile
{
    private TileObject _activeTileObject;

    [SerializeField] private Particle[] _particles;
    private Dictionary<ParticleName, Particle> _particleDict;

    protected override void OnAwake()
    {
        base.OnAwake();

        _particleDict = new Dictionary<ParticleName, Particle>();

        foreach (var particle in _particles)
        {
            if(!_particleDict.ContainsKey(particle.particleName)){
                _particleDict[particle.particleName] = particle;
            }
        }
    }

    public override TileObjectType GetTileType()
    {
        return _activeTileObject.Type;
    }

    public override TileObjectCategory GetTileCategory()
    {
        return _activeTileObject.Category;
    }

    public override void Init(int col, int row)
    {
        _tilePos = new Vector2Int(col, row);
        _tileId = col * GridManager.s_Instance.GridDimension + row;
    }

    public override void SetTileObject(TileObject tileObject)
    {
        if(tileObject != null)
        {
            if(_activeTileObject != null) {
                if(_activeTileObject.Type == TileObjectType.None) TileObjectGenerator.s_Instance.ReturnTileObject(_activeTileObject);
                _activeTileObject.ParentTile = null;
            }
            _activeTileObject = tileObject;
            _activeTileObject.ParentTile = this;
            _activeTileObject.transform.position = transform.position;
        }
    }

    public override void SetTileObject(TileObjectType tileObjectType)
    {
        TileObject tileObject = TileObjectGenerator.s_Instance.GetTileObject(tileObjectType);
        SetTileObject(tileObject);
    }

    public override TileObject ActiveTileObject()
    {
        return _activeTileObject;
    }

    public override void DestroyTileObject()
    {
        if(_activeTileObject != null || _activeTileObject.Type != TileObjectType.None || _activeTileObject.Type != TileObjectType.Absent)
        {
            TileObjectGenerator.s_Instance.ReturnTileObject(_activeTileObject);
            _activeTileObject = TileObjectGenerator.s_Instance.GetTileObject(TileObjectType.None);
        } 
    }

    public override void PlayParticle(ParticleName particleName)
    {
        _particleDict[particleName].Play();
    }
}