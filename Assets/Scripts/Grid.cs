using System;
using System.Collections.Generic;
using System.Linq;
using Flap;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid
{
    [SerializeField] private int _playFieldDimension;
    [SerializeField] private bool[,] _occcupiedPositions2d;
    [SerializeField] private int[] _occcupiedPositions;

    [SerializeField] private int[,] _columnRowPositions;
    private Dictionary<int, Queue<TileObjectType>> _tileObjPool;
    [SerializeField] private GameObject[] _blockPrefabs;

    // private float[,] _gridArray;

    public int PlayFieldDimension { get => _playFieldDimension; }
    public bool[,] OcccupiedPositions2d { get => _occcupiedPositions2d; }
    public Dictionary<int, Queue<TileObjectType>> TileObjPool { get => _tileObjPool; }

    public Grid(GridBlueprint gridBP)
    {
        _tileObjPool = new Dictionary<int, Queue<TileObjectType>>();

        _playFieldDimension = gridBP.PlayFieldDimension;
        _occcupiedPositions = gridBP.OcccupiedPositions;
        _occcupiedPositions2d = OccupiedPositionsTo2dArray(gridBP.OcccupiedPositions);
        _tileObjPool = gridBP.GetTileObjectQueue();

        _columnRowPositions = new int[_playFieldDimension, _playFieldDimension];
    }

    private bool[,] OccupiedPositionsTo2dArray(int[] occupiedPos)
    {
        int dimension = LevelManager.GridDimension;
        bool[,] occupiedPosMap = new bool[dimension, dimension];

        for(int col = 0; col < occupiedPosMap.GetLength(0); col++)
        {
            for(int row = 0; row < occupiedPosMap.GetLength(1); row++)
            {
                occupiedPosMap[col, row] = false;
            }
        }

        foreach (int pos in occupiedPos)
        {
            int row = pos % dimension;
            int col = pos / dimension;

            occupiedPosMap[col, row] = true;
        }

        return occupiedPosMap;

    }

    public void RandomFillTiles()
    {   
        foreach (int tile in _occcupiedPositions)
        {
            var tileTypes = LevelManager.s_Instance.TileObjPrefabMap.Keys.ToList();
            tileTypes.Remove(TileObjectType.None);
            TileObjectType type = tileTypes[Random.Range(0, tileTypes.Count)];
            LevelManager.s_Instance.GetTile(tile).SetTile(type);
        }
    }

    public (List<int>, List<int>) ClickTile(int tileNumber)
    {
        if(!LevelManager.s_Instance.GetTile(tileNumber).ActiveTileObject().Clickable) return (null, null);

        List<int> connectedTiles = new List<int>();
        List<int> hitTiles = new List<int>();
        GetConnectedTiles(tileNumber, ref connectedTiles, ref hitTiles);
        string str = "Cliked at tile: " + tileNumber + ", connected tiles are: ";
        foreach (int tile in connectedTiles)
        {
            str += tile + ", ";
        }
        Debug.Log(str);

        string strHittable = "Cliked at tile: " + tileNumber + ", hittable tiles are: ";
        foreach (int tile in hitTiles)
        {
            strHittable += tile + ", ";
        }
        Debug.Log(strHittable);

        return (connectedTiles, hitTiles);
    }

    private void GetConnectedTiles(int tile, ref List<int> connectedTiles, ref List<int> hittableTilesOnEdge, int previousTile = -1)
    {
        List<int> adjacentTiles = GetAdjacentTiles(tile);

        if(previousTile == -1)
        {
            connectedTiles.Add(tile);
        }
        
        foreach (int adjacentTile in adjacentTiles)
        {
            if(adjacentTile == previousTile)
            {
                continue;
            }
            TileObjectType selfType = LevelManager.s_Instance.GetTile(tile).GetTileType();
            TileObjectType adjacentType = LevelManager.s_Instance.GetTile(adjacentTile).GetTileType();
            if(selfType == adjacentType && adjacentType != TileObjectType.Absent)
            {
                if(!connectedTiles.Contains(adjacentTile))
                {
                    connectedTiles.Add(adjacentTile);
                    GetConnectedTiles(adjacentTile, ref connectedTiles, ref hittableTilesOnEdge, tile);
                }
            }
            else if(LevelManager.s_Instance.GetTile(adjacentTile).GetTileCategory().HasFlag(TileObjectCategory.HittableTileObject))
            {
                hittableTilesOnEdge.Add(adjacentTile);
            }
        }

        return;
    }

    private List<int> GetAdjacentTiles(int tile)
    {
        int dimension = LevelManager.GridDimension;
        List<int> adjacentTiles = new List<int>();

        // Calculate row and column of the given tile
        int row = tile % dimension;
        int col = tile / dimension;

        // Check above (row + 1)
        if (row + 1 < dimension && LevelManager.s_Instance.GetTile(tile + 1).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile + 1);
        }

        // Check below (row - 1)
        if (row - 1 >= 0 && LevelManager.s_Instance.GetTile(tile -1).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile -1);
        }

        // Check right (col + 1)
        if (col + 1 < dimension && LevelManager.s_Instance.GetTile(tile + dimension).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile + dimension);
        }

        // Check left (col - 1)
        if (col - 1 >= 0 && LevelManager.s_Instance.GetTile(tile - dimension).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile - dimension);
        }

        return adjacentTiles;
    }

    private void FillEmptyTiles()
    {

    }

}
