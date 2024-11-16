using System;
using System.Collections;
using System.Collections.Generic;
using Flap;
using UnityEngine;
using System.Linq;

using Random = UnityEngine.Random;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    public static GridManager s_Instance;

    public UnityAction OnFillEnd;

    [SerializeField] private GridBlueprint _gridBlueprint;

    [SerializeField] private int _gridDimension;
    [SerializeField] private bool[,] _occcupiedPositions2d;
    [SerializeField] private int[] _occcupiedPositions;

    private Dictionary<int, Queue<TileObjectType>> _tileObjPool;

    [SerializeField] private GameObject _presentTilePrefab;
    [SerializeField] private GameObject _absentTilePrefab;

    [SerializeField] private RectTransform _playfieldRectTransform;

    [SerializeField] private GameSettings _gameSettings;

    private int _runningSequenceCount;
    private bool _tileDestroyed;
    
    private int _fallSequenceCount;
    private int _preFallSequenceCount;

    private Tile[,] _tileMap;

    private Rect _playFieldRect;

    private float _tileWidth;
    private float _tileHeight;

    public static int GridDimension { get => s_Instance._gridDimension; }
    public Tile[,] TileMap { get => _tileMap; }
    public int RunningSequences { get => _runningSequenceCount; set => _runningSequenceCount = value; }
    public float TileWidth { get => _tileWidth; }
    public float TileHeight { get => _tileHeight; }

    #region Utility functions

    public static IEnumerator MoveTileObjectToPosition(TileObject tileObject, Vector3 targetPosition, TileMoveAnimation animation)
    {
        float elapsedTime = 0f;

        Vector3 startPosition = tileObject.transform.position;

        Vector3 initialScale = tileObject.transform.localScale;

        while (elapsedTime < animation.blockToGoalDuration)
        {
            // Calculate normalized time [0, 1]
            float normalizedTime = elapsedTime / animation.blockToGoalDuration;

            // Interpolate position
            float positionFactor = animation.blockToGoalMoveCurve.Evaluate(normalizedTime);
            tileObject.transform.position = Vector3.Lerp(startPosition, targetPosition, positionFactor);

            // Interpolate scale
            float scaleFactor = animation.blockToGoalScaleCurve.Evaluate(normalizedTime);
            tileObject.transform.localScale = initialScale * scaleFactor;

            // Increment time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the final position and scale are set
        tileObject.transform.position = targetPosition;
        tileObject.transform.localScale = initialScale * animation.blockToGoalScaleCurve.Evaluate(1f);
    }

    public static Vector2 GridToWorldPosition(int col, int row)
    {
        float x = s_Instance._playFieldRect.xMin + s_Instance._tileWidth / 2 + col * s_Instance._tileWidth;
        float y = s_Instance._playFieldRect.yMin + s_Instance._tileHeight / 2 + row * s_Instance._tileHeight ;

        return new Vector2(x, y);
    }

    public static void GetConnectedTiles(int tile, ref List<int> connectedTiles, ref List<int> hittableTilesOnEdge, int previousTile = -1)
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
            TileObjectType selfType = s_Instance.GetTile(tile).GetTileType();
            TileObjectType adjacentType = s_Instance.GetTile(adjacentTile).GetTileType();
            if(selfType == adjacentType && adjacentType != TileObjectType.Absent)
            {
                if(!connectedTiles.Contains(adjacentTile))
                {
                    connectedTiles.Add(adjacentTile);
                    GetConnectedTiles(adjacentTile, ref connectedTiles, ref hittableTilesOnEdge, tile);
                }
            }
            else if(s_Instance.GetTile(adjacentTile).GetTileCategory().HasFlag(TileObjectCategory.HitableTileObject))
            {
                hittableTilesOnEdge.Add(adjacentTile);
            }
        }

        return;
    }

    public static List<int> GetAdjacentTiles(int tile)
    {
        List<int> adjacentTiles = new List<int>();

        // Calculate row and column of the given tile
        int row = tile % GridDimension;
        int col = tile / GridDimension;

        // Check above (row + 1)
        if (row + 1 < GridDimension && s_Instance.GetTile(tile + 1).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile + 1);
        }

        // Check below (row - 1)
        if (row - 1 >= 0 && s_Instance.GetTile(tile -1).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile -1);
        }

        // Check right (col + 1)
        if (col + 1 < GridDimension && s_Instance.GetTile(tile + GridDimension).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile + GridDimension);
        }

        // Check left (col - 1)
        if (col - 1 >= 0 && s_Instance.GetTile(tile - GridDimension).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile - GridDimension);
        }

        return adjacentTiles;
    }

    public static int TilePosToId(Vector2Int pos)
    {
        return GridDimension * pos.x + pos.y;
    }

    public static Vector2Int TileIdToPos(int id)
    {
        int col = id / s_Instance._gridDimension;
        int row = id % s_Instance._gridDimension;
        return new Vector2Int(col, row);
    }

    #endregion

    void Awake()
    {

        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this); 
        }
        else 
        { 
            s_Instance = this; 
        }

        _tileObjPool = new Dictionary<int, Queue<TileObjectType>>();

        _occcupiedPositions = _gridBlueprint.OcccupiedPositions;
        _occcupiedPositions2d = OccupiedPositionsTo2dArray(_gridBlueprint.OcccupiedPositions);
        _tileObjPool = _gridBlueprint.GetTileObjectQueue();

        _tileMap = new Tile[_gridDimension, _gridDimension];

        _runningSequenceCount = 0;

        _fallSequenceCount = 0;
        _preFallSequenceCount = _fallSequenceCount;

    }

    void Start()
    {
        GenerateTileMap();
        RandomFillTiles();
        InputManager.s_Instance.ConnectInput(this);
    }

    void FixedUpdate()
    {
        if(_tileDestroyed && _runningSequenceCount <= 0)
        {
            FillEmptyTiles();
        }
        _tileDestroyed = false;

        if(_fallSequenceCount <= 0 && _fallSequenceCount != _preFallSequenceCount)
        {
            OnFillEnd?.Invoke();
        }
        _preFallSequenceCount = _fallSequenceCount;
    }

    public void RandomFillTiles()
    {   
        foreach (int tile in _occcupiedPositions)
        {
            var tileTypes = TileObjectGenerator.s_Instance.TileObjPrefabMap.Keys.ToList();
            tileTypes.Remove(TileObjectType.None);
            TileObjectType type = tileTypes[Random.Range(0, tileTypes.Count)];
            GetTile(tile).SetTile(type);
        }
    }

    private bool[,] OccupiedPositionsTo2dArray(int[] occupiedPos)
    {
        int dimension = GridDimension;
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

    void GenerateTileMap()
    {
        _playFieldRect = GetWorldSpaceRect(_playfieldRectTransform);
        _tileWidth = _playFieldRect.width / _gridDimension;
        _tileHeight = _playFieldRect.height / _gridDimension;

        Vector2 startPosition = new Vector2(
            _playFieldRect.xMin + _tileWidth / 2,
            _playFieldRect.yMin + _tileHeight / 2
        );

        for(int col = 0; col < _tileMap.GetLength(0); col++)
        {
            for(int row = 0; row < _tileMap.GetLength(1); row++)
            {
                GameObject tileGo;

                if(_occcupiedPositions2d[col, row]){
                    tileGo = Instantiate(_presentTilePrefab, transform);
                }
                else{
                    tileGo = Instantiate(_absentTilePrefab, transform);
                }

                tileGo.transform.localScale = new Vector3(_tileWidth, _tileHeight, 1);
    
                Vector2 tilePosition = new Vector3(
                    startPosition.x + col * _tileWidth,
                    startPosition.y + row * _tileHeight,
                    0.0f
                );

                tileGo.transform.position = tilePosition;
                tileGo.name = "(" + col + ", " + row + ")";

                try{ _tileMap[col, row] = tileGo.GetComponent<Tile>(); }
                catch(Exception e){ Debug.LogError(e); }

                _tileMap[col, row].Init(col, row);
            }
        }
    }

    Rect GetWorldSpaceRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // Bottom-left corner
        Vector3 bottomLeft = corners[0];
        // Top-right corner
        Vector3 topRight = corners[2];

        float width = Mathf.Abs(topRight.x - bottomLeft.x);
        float height = Mathf.Abs(topRight.y - bottomLeft.y);

        return new Rect(bottomLeft.x, bottomLeft.y, width, height);
    }

    public void OnTapInput(Vector2 touchScreenPosition)
    {
        if(_runningSequenceCount > 0 || _fallSequenceCount > 0) return;
        Ray ray = Camera.main.ScreenPointToRay(touchScreenPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if(hit && hit.collider.CompareTag("Tile"))
        {
            Tile tile = hit.collider.transform.GetComponent<Tile>();
            if(tile.GetTileCategory().HasFlag(TileObjectCategory.ClickableTileObject))
            {
                ClickableTileObject tileObject = (ClickableTileObject)tile.ActiveTileObject();
                tileObject.OnClick();
            }
        }
    }

    public void FillEmptyTiles()
    {
        for(int col = 0; col < _gridDimension; col++)
        {
            for(int row = 0; row < _gridDimension; row++)
            {
                Tile tile = _tileMap[col, row];
                if(tile.GetTileType() == TileObjectType.None)
                {
                    // Debug.Log("Detected empty tile starting recursive algorithm. Empty tile is: " + tile.TilePos);
                    FillColumn(tile); // This will recursively fill the empty tiles.
                    break;
                }
            }
        }
    }

    void FillColumn(Tile tile)
    {
        if(tile.GetTileType() == TileObjectType.None)
        {
            if(tile.GetTileType() == TileObjectType.Absent)
            {
                if(tile.TilePos.y + 1 >= _gridDimension) return;
                else FillColumn(_tileMap[tile.TilePos.x, tile.TilePos.y + 1]);
            }

            // Debug.Log("Recursive algorithm started tile (" + tile.TilePos + ") seems to be empty attempting to fill. ");
            bool foundTileInGrid = false;
            for(int row = tile.TilePos.y + 1; row < _gridDimension; row++)
            {
                Tile tileAbove = _tileMap[tile.TilePos.x, row];
                if(tileAbove.GetTileType() != TileObjectType.None && tileAbove.GetTileType() != TileObjectType.Absent) // Change with tile.CanFall()
                {
                    foundTileInGrid = true;
                    TileObject tileObject = tileAbove.ActiveTileObject();
                    tileAbove.SetTile(TileObjectType.None);
                    StartCoroutine(FallToPosition(tileObject, tile));
                    // tile.SetTile(tileAbove.GetTileType());
                    break;
                }
            }

            if(!foundTileInGrid && _tileObjPool.ContainsKey(tile.TilePos.x) && _tileObjPool[tile.TilePos.x].Count > 0)
            {
                TileObjectType type = _tileObjPool[tile.TilePos.x].Dequeue();
                TileObject tileObject = TileObjectGenerator.s_Instance.GetTileObject(type);
                Vector2 tilePos = GetTile(new Vector2Int(tile.TilePos.x, _gridDimension - 1)).transform.position;
                tilePos.y += _tileHeight * 2;
                tileObject.transform.position = tilePos;
                StartCoroutine(FallToPosition(tileObject, tile));
            }

        }

        if(tile.TilePos.y + 1 >= _gridDimension) return;
        else FillColumn(_tileMap[tile.TilePos.x, tile.TilePos.y + 1]);

    }

    IEnumerator FallToPosition(TileObject tileObject, Tile tile)
    {
        _fallSequenceCount++;

        yield return StartCoroutine(MoveTileObjectToPosition(tileObject, GridToWorldPosition(tile.TilePos.x, tile.TilePos.y), LevelManager.Settings.FallAnimation));
        tile.SetTile(tileObject);

        _fallSequenceCount--;
    }

    public Tile GetTile(Vector2Int tilePos)
    {
        if(tilePos.x >= _gridDimension || tilePos.y >= _gridDimension ||
        tilePos.x < 0 || tilePos.y < 0)
        {
            return null;
        }
        return _tileMap[tilePos.x, tilePos.y];
    }

    public Tile GetTile(int tileNum)
    {
        if(tileNum >= _gridDimension * _gridDimension || tileNum < 0){
            return null;
        }
        return _tileMap[tileNum / _gridDimension, tileNum % _gridDimension];
    }

    public void OnTileDestroy(Tile tile, TileObject tileObject)
    {
        _tileDestroyed = true;

        if(tileObject.Category.HasFlag(TileObjectCategory.AudibleTileObject)){
            AudioName audio = (tileObject as IAudible).GetAudioName();
            AudioManager.s_Instance.Play(audio);
        }

        if(tileObject.Category.HasFlag(TileObjectCategory.ParticleEmittingTileobject)){
            tile.PlayParticle((tileObject as IParticleEmitting).GetParticleName());
        }

        if(LevelManager.s_Instance.CountsTowardsGoal(tileObject)){
            tile.SetTile(TileObjectType.None);
        }
        else{
            tile.DestroyTileObject();
        }
    }

}
