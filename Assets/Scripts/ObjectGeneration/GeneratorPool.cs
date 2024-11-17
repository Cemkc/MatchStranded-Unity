using System.Collections.Generic;
using UnityEngine;

public class GeneratorPool : TileObjectGenerator
{
    [SerializeField] private int _maxDimensionCoverage;
    private Dictionary<TileObjectType, Stack<GameObject>> _tileObjectsDict;

    public override void Init()
    {
        int dimension = Mathf.Min(GridManager.GridDimension, _maxDimensionCoverage); 
        int numberOfObjectsPerType = dimension * dimension * 2;

        _tileObjectsDict = new Dictionary<TileObjectType, Stack<GameObject>>();

        foreach(KeyValuePair<TileObjectType, GameObject> tileObjecPrefab in _tileObjPrefabMap)
        {
            if(tileObjecPrefab.Key == TileObjectType.Absent) continue;

            if(!_tileObjectsDict.ContainsKey(tileObjecPrefab.Key)){
                _tileObjectsDict[tileObjecPrefab.Key] = new Stack<GameObject>();
            }

            for(int i = 0; i < numberOfObjectsPerType; i++){
                GameObject tileGo = Instantiate(tileObjecPrefab.Value, transform);
                ResetTileObject(tileGo);
                _tileObjectsDict[tileObjecPrefab.Key].Push(tileGo);
                tileGo.SetActive(false);
            }
        }
    }

    public override TileObject GetTileObject(TileObjectType type)
    {
        if(_tileObjectsDict[type].Count <= 0)
        {
            Debug.Log("Created object dynamically.");
            GameObject tileDynamicGo = Instantiate(_tileObjPrefabMap[type], transform);
            return tileDynamicGo.GetComponent<TileObject>();
        }
        GameObject tileGo = _tileObjectsDict[type].Pop();
        ResetTileObject(tileGo);
        tileGo.SetActive(true);
        TileObject tileObject;
        if(tileGo.TryGetComponent(out tileObject)){
            return tileObject;
        }
        else return null;
    }

    public override void ReturnTileObject(TileObject tileObject)
    {
        TileObjectType type = tileObject.Type;
        ResetTileObject(tileObject.gameObject);
        tileObject.gameObject.SetActive(false);
        _tileObjectsDict[type].Push(tileObject.gameObject);
    }

    private void ResetTileObject(GameObject tileGo)
    {
        tileGo.transform.position = Vector3.zero;
        tileGo.transform.rotation = Quaternion.identity;
        tileGo.transform.localScale = new Vector3(GridManager.s_Instance.TileWidth * 0.8f, GridManager.s_Instance.TileHeight * 0.8f, 1);
    }

}
       
