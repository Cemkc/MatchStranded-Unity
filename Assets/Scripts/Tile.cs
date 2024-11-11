using System.Collections.Generic;
using UnityEngine;

namespace Flap{
    public class Tile : MonoBehaviour
    {
        TileObject activeTileObject;

        public TileObject ActiveTileObject { get => activeTileObject; }

        public void Init(List<GameObject> tileObjectList)
        {
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
            if(activeTileObject != null && type == activeTileObject.GetTileObjType()) return;

            TileObject[] tileObjects = GetComponentsInChildren<TileObject>(includeInactive: true);
            foreach (TileObject tileObject in tileObjects)
            {
                if(tileObject.GetTileObjType() == type)
                {
                    if(activeTileObject != null) activeTileObject.gameObject.SetActive(false);
                    activeTileObject = tileObject;
                    activeTileObject.gameObject.SetActive(true);
                } 
            }
            
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
