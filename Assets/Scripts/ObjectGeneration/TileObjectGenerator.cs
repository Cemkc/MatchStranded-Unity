using System;
using System.Collections.Generic;
using UnityEngine;

public enum BlockGenerationMethod
{
    None,
    Dynamic,
    ObjectPooling
}

[Serializable]
public struct TilePrefabAttribution
{
    public TileObjectType tileObjType;
    public GameObject prefab;
}

public abstract class TileObjectGenerator : MonoBehaviour
{
    public static TileObjectGenerator s_Instance;

    public TilePrefabAttribution[] _tilePrefabAttribution;
    protected Dictionary<TileObjectType, GameObject> _tileObjPrefabMap;

    public Dictionary<TileObjectType, GameObject> TileObjPrefabMap { get => _tileObjPrefabMap; }

    void Awake()
    {
        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this); 
        }
        else 
        { 
            s_Instance = this; 
        }

        _tileObjPrefabMap = new Dictionary<TileObjectType, GameObject>();
        foreach (TilePrefabAttribution attrib in _tilePrefabAttribution)
        {
            _tileObjPrefabMap[attrib.tileObjType] = attrib.prefab;
        }

        Init();

    }

    public abstract void Init();
    public abstract TileObject GetTileObject(TileObjectType type);
    public abstract void ReturnTileObject(TileObject tileObject);
}