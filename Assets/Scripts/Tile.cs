using System.Collections.Generic;
using UnityEngine;

namespace Flap{
    public class Tile : MonoBehaviour
    {
        private int _tileId;
        private TileObject _activeTileObject;

        public int TileId { get => _tileId; }

        public void Init(int id, List<GameObject> tileObjectList)
        {
            _tileId = id;
            foreach (GameObject tileObject in tileObjectList)
            {
                GameObject go = Instantiate(tileObject, transform);
                go.transform.localPosition = new Vector3(0f, 0f, -1f);
                go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                go.SetActive(false);
            }
        }

        public void SetTile(TileObjType type)
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

        public TileObjType GetTileType()
        {
            if(_activeTileObject == null) return TileObjType.None;

            return _activeTileObject.GetTileObjType();
        }

        public void FillRandom()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
