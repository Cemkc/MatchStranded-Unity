using UnityEngine;
using Flap;

public class PresentTile : Tile
{
    public override void Init(int col, int row)
    {
        _activeTileObject = null;
        _tilePos = new Vector2Int(col, row);

        foreach (GameObject tileObject in LevelManager.s_Instance.TileObjPrefabMap.Values)
        {
            GameObject go = Instantiate(tileObject, transform);
            go.transform.localPosition = new Vector3(0f, 0f, -1f);
            go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            go.SetActive(false);
        }
    }

    public override void SetTile(TileObjType type)
    {
        if((_activeTileObject != null && type == _activeTileObject.GetTileObjType()) ||
        (_activeTileObject == null && type == TileObjType.None))
        {
            return;
        }

        if(type == TileObjType.None)
        {
            _activeTileObject.gameObject.SetActive(false);
            _activeTileObject = null;
            return;
        }

        TileObject[] tileObjects = GetComponentsInChildren<TileObject>(includeInactive: true);
        foreach (TileObject tileObject in tileObjects)
        {
            if(tileObject.GetTileObjType() == type)
            {
                if(_activeTileObject != null) _activeTileObject.gameObject.SetActive(false);
                _activeTileObject = tileObject;
                _activeTileObject.gameObject.SetActive(true);
            } 
        }
    }

    public override TileObjType GetTileObjType()
    {
        if(_activeTileObject == null) return TileObjType.None;

        return _activeTileObject.GetTileObjType();
    }

    public override void OnHit()
    {

    }

}