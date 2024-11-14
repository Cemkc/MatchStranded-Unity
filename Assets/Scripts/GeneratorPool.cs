
using UnityEngine;

public class GeneratorPool : TileObjectGenerator
{
    public override void Init()
    {
        foreach(TileObjectType tileObjectType in _tileObjPrefabMap.Keys)
        {
            
        }
    }

    public override TileObject GetTileObject(TileObjectType type)
    {
        throw new System.NotImplementedException();
    }

    public override void ReturnTileObject(TileObject tileObject)
    {
        throw new System.NotImplementedException();
    }
}
       
