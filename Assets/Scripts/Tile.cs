using System.Collections.Generic;
using UnityEngine;

namespace Flap{
    public abstract class Tile : MonoBehaviour
    {
        protected Vector2Int _tilePos;
        protected int _tileId;

        public Vector2Int TilePos { get => _tilePos; }
        public int TileId { get => _tileId; }

        public abstract void Init(int col, int row);
        public abstract void SetTile(TileObjectType type);
        public abstract TileObject ActiveTileObject();
        public abstract TileObjectType GetTileType();
        public abstract TileObjectCategory GetTileCategory();
        public abstract void OnHit();
    }
}
