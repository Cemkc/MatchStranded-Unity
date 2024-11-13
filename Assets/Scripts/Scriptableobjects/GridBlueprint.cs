using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Blueprint", menuName = "Scriptable Objects / Grid Blueprint")]
public class GridBlueprint : ScriptableObject
{
    [Serializable]
    struct TileObjListStruct
    {
        public int column;
        public List<TileObjectType> tileObjects;
    }

    [SerializeField] private int _playFieldDimension;
    [SerializeField] private int[] _occcupiedPositions;
    [SerializeField] private List<TileObjListStruct> _tileObjectQueueInspector; // Temporary solution
    private Dictionary<int, Queue<TileObjectType>> _tileObjectQueue = new Dictionary<int, Queue<TileObjectType>>();

    public int PlayFieldDimension { get => _playFieldDimension; }
    public int[] OcccupiedPositions { get => _occcupiedPositions; }

    public Dictionary<int, Queue<TileObjectType>> GetTileObjectQueue() // Make it an initializer don't do this each time to get the queue
    {
        foreach(var tileObjColumnAttrib in _tileObjectQueueInspector)
        {
            if(!_tileObjectQueue.ContainsKey(tileObjColumnAttrib.column)) {
                _tileObjectQueue[tileObjColumnAttrib.column] = new Queue<TileObjectType>(tileObjColumnAttrib.tileObjects);
            }
        }

        return _tileObjectQueue;
    }
}
