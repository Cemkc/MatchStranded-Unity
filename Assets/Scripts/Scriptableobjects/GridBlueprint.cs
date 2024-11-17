using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Grid Blueprint", menuName = "Scriptable Objects/Level/Grid Blueprint")]
public class GridBlueprint : ScriptableObject
{
    public UnityAction<GridBlueprint> OnDraw;
    public UnityAction<GridBlueprint> OnInit;

    [Serializable]
    struct TileObjListStruct
    {
        public int column;
        public List<TileObjectType> tileObjects;
    }

    [SerializeField] private int _dimension;
    [SerializeField] private TileObjectType[] _occcupiedPositions;
    // private List<TileObjListStruct> _tileObjectQueueInspector; // Temporary solution
    // private Dictionary<int, Queue<TileObjectType>> _tileObjectQueue = new Dictionary<int, Queue<TileObjectType>>();

    public int Dimension { get => _dimension; }
    public TileObjectType[] OcccupiedPositions { get => _occcupiedPositions; }

    public void Init()
    {
        _occcupiedPositions = new TileObjectType[_dimension * _dimension];
        OnInit?.Invoke(this);
    }

    public void Draw()
    {
        OnDraw?.Invoke(this);
    }

}
