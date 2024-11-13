using UnityEngine;
using Flap;

public class PresentTile : Tile
{
    private TileObject _activeTileObject;

    public override TileObjType GetTileType()
    {
        return _activeTileObject.Type;
    }

    public override void Init(int col, int row)
    {
        _tilePos = new Vector2Int(col, row);

        foreach (GameObject tileObject in LevelManager.s_Instance.TileObjPrefabMap.Values)
        {
            GameObject go = Instantiate(tileObject, transform);
            go.transform.localPosition = new Vector3(0f, 0f, -1f);
            go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            _activeTileObject = go.GetComponentInChildren<EmptyTile>();
            go.SetActive(false);
        }
    }

    public override void SetTile(TileObjType type)
    {
        if(_activeTileObject != null && type == _activeTileObject.Type)
        {
            return;
        }

        TileObject[] tileObjects = GetComponentsInChildren<TileObject>(includeInactive: true);
        foreach (TileObject tileObject in tileObjects)
        {
            if(tileObject.Type == type)
            {
                if(_activeTileObject != null) _activeTileObject.gameObject.SetActive(false);
                _activeTileObject = tileObject;
                _activeTileObject.ParentTile = this;
                _activeTileObject.gameObject.SetActive(true);
            } 
        }
    }

    public override TileObject ActiveTileObject()
    {
        return _activeTileObject;
    }

    public override void OnHit()
    {

    }
}