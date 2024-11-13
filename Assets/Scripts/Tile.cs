using System.Collections.Generic;
using UnityEngine;

namespace Flap{
    public abstract class Tile : MonoBehaviour
    {
        protected TileObject _activeTileObject;
        protected Vector2Int _tilePos;

        public Vector2Int TilePos { get => _tilePos; }

        public abstract void Init(int col, int row);
        public abstract void SetTile(TileObjType type);
        public abstract TileObjType GetTileObjType();
        public abstract void OnHit();
    }
}
