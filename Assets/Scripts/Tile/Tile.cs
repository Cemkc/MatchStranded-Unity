using UnityEngine;

namespace Flap{
    public abstract class Tile : MonoBehaviour
    {
        protected Vector2Int _tilePos;
        protected int _tileId;

        void Awake(){
            OnAwake();
        }

        protected virtual void OnAwake(){}

        public Vector2Int TilePos { get => _tilePos; }
        public int TileId { get => _tileId; }

        public abstract void Init(int col, int row);
        public abstract void SetTileObject(TileObject tileObject);
        public abstract void SetTileObject(TileObjectType tileObjectType);
        public abstract void DestroyTileObject();

        public abstract TileObject ActiveTileObject();
        public abstract TileObjectType GetTileType();
        public abstract TileObjectCategory GetTileCategory();

        public abstract void PlayParticle(ParticleName particleName);
    }
}
