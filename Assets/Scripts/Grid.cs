using System;
using System.Collections.Generic;
using System.Linq;
using Flap;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid
{
    [SerializeField] private int _playFieldDimension;
    [SerializeField] private bool[,] _occcupiedPositions2d;
    [SerializeField] private int[] _occcupiedPositions;

    [SerializeField] private int[,] _columnRowPositions;
    private Dictionary<int, Queue<TileObjType>> _tileObjPool;
    [SerializeField] private GameObject[] _blockPrefabs;

    // private float[,] _gridArray;

    public int PlayFieldDimension { get => _playFieldDimension; }
    public bool[,] OcccupiedPositions2d { get => _occcupiedPositions2d; }

    public Grid(GridBlueprint gridBP)
    {
        _tileObjPool = new Dictionary<int, Queue<TileObjType>>();

        _playFieldDimension = gridBP.PlayFieldDimension;
        _occcupiedPositions = gridBP.OcccupiedPositions;
        _occcupiedPositions2d = OccupiedPositionsTo2dArray(gridBP.OcccupiedPositions);

        _columnRowPositions = new int[_playFieldDimension, _playFieldDimension];
    }

    private bool[,] OccupiedPositionsTo2dArray(int[] occupiedPos)
    {
        int dimension = LevelManager.s_Instance.GridDimension;
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
            TileObjType type = tileTypes[Random.Range(0, tileTypes.Count)];
            LevelManager.s_Instance.GetTile(tile).SetTile(type);
        }
    }

    public void ClickTile(int tileNumber)
    {
        List<int> connectedTiles = new List<int>();
        GetConnectedTiles(tileNumber, ref connectedTiles);
        string str = "Cliked at tile: " + tileNumber + ", connected tiles are: ";
        foreach (int tile in connectedTiles)
        {
            str += tile + ", ";
        }
        Debug.Log(str);

        if(connectedTiles.Count > 1)
        {
            foreach (int tileNum in connectedTiles)
            {
                LevelManager.s_Instance.GetTile(tileNum).SetTile(TileObjType.None);
            }

            LevelManager.s_Instance.FillEmptyTiles();
        }

    }

    private void GetConnectedTiles(int tile, ref List<int> connectedTiles, int previousTile = -1)
    {
        List<int> adjacentTiles = GetAdjacentTiles(tile);
        string str = "Adjacent Tiles Are: ";
        foreach (var adjTile in adjacentTiles)
        {
            str += adjTile + ", ";
        }
        Debug.Log(str);

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
            TileObjType selfType = LevelManager.s_Instance.GetTile(tile).GetTileObjType();
            TileObjType adjacentType = LevelManager.s_Instance.GetTile(adjacentTile).GetTileObjType();
            if(selfType == adjacentType && adjacentType != TileObjType.Absent)
            {
                if(!connectedTiles.Contains(adjacentTile))
                {
                    connectedTiles.Add(adjacentTile);
                    GetConnectedTiles(adjacentTile, ref connectedTiles, tile);
                }
            }
        }

        return;
    }

    private List<int> GetAdjacentTiles(int tile)
    {
        int dimension = LevelManager.s_Instance.GridDimension;
        List<int> adjacentTiles = new List<int>();

        // Calculate row and column of the given tile
        int row = tile % dimension;
        int col = tile / dimension;

        // Check above (row + 1)
        if (row + 1 < dimension && LevelManager.s_Instance.GetTile(tile + 1).GetTileObjType() != TileObjType.Absent)
        {
            adjacentTiles.Add(tile + 1);
        }

        // Check below (row - 1)
        if (row - 1 >= 0 && LevelManager.s_Instance.GetTile(tile -1).GetTileObjType() != TileObjType.Absent)
        {
            adjacentTiles.Add(tile -1);
        }

        // Check right (col + 1)
        if (col + 1 < dimension && LevelManager.s_Instance.GetTile(tile + dimension).GetTileObjType() != TileObjType.Absent)
        {
            adjacentTiles.Add(tile + dimension);
        }

        // Check left (col - 1)
        if (col - 1 >= 0 && LevelManager.s_Instance.GetTile(tile - dimension).GetTileObjType() != TileObjType.Absent)
        {
            adjacentTiles.Add(tile - dimension);
        }

        return adjacentTiles;
    }

    private void FillEmptyTiles()
    {

    }

}
