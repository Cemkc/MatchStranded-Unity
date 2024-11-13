using System;
using System.Collections;
using System.Collections.Generic;
using Flap;
using UnityEngine;

[Serializable]
public struct TilePrefabAttribution
{
    public TileObjType tileObjType;
    public GameObject prefab;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager s_Instance;

    [SerializeField] private int _gridDimension;
    [SerializeField] private GridBlueprint _gridBlueprint;
    private Grid _grid;
    [SerializeField] private GameObject _presentTilePrefab;
    [SerializeField] private GameObject _absentTilePrefab;

    [SerializeField] private TilePrefabAttribution[] _tilePrefabAttribution;

    private Dictionary<TileObjType, GameObject> _tileObjPrefabMap;

    private Tile[,] _tileMap;

    private Rect _playField;

    private float _tileWidth;
    private float _tileHeight;

    public int GridDimension { get => _gridDimension; }
    public Tile[,] TileMap { get => _tileMap; }
    public Dictionary<TileObjType, GameObject> TileObjPrefabMap { get => _tileObjPrefabMap; set => _tileObjPrefabMap = value; }

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

        _tileMap = new Tile[_gridDimension, _gridDimension];
        _grid = new Grid(_gridBlueprint);
        
        _tileObjPrefabMap = new Dictionary<TileObjType, GameObject>();
        foreach (TilePrefabAttribution attrib in _tilePrefabAttribution)
        {
            _tileObjPrefabMap[attrib.tileObjType] = attrib.prefab;
        }

    }

    void Start()
    {
        GenerateTileMap();
        _grid.RandomFillTiles();
        InputManager.s_Instance.ConnectInput(this);
    }

    void Update()
    {
        
    }

    void GenerateTileMap()
    {
        SetPlayField();

        Vector2 startPosition = new Vector2(
            _playField.xMin + _tileWidth / 2,
            _playField.yMin + _tileHeight / 2
        );

        for(int col = 0; col < _tileMap.GetLength(0); col++)
        {
            for(int row = 0; row < _tileMap.GetLength(0); row++)
            {
                GameObject tileGo;

                if(_grid.OcccupiedPositions2d[col, row]){
                    tileGo = Instantiate(_presentTilePrefab, transform);
                }
                else{
                    Debug.Log("Found absent on row: " + row + ", column: " + col);
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

    void SetPlayField()
    {
        // Calculate screen dimensions in Unity units using an orthographic camera
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Define the play field dimensions as a portion of the screen (e.g., 80%)
        float playFieldWidth = screenWidth * 0.8f;
        float playFieldHeight = playFieldWidth;

        // Center the play field in the middle of the screen
        _playField = new Rect(
            -playFieldWidth / 2,  // x position
            -playFieldHeight / 2, // y position
            playFieldWidth,
            playFieldHeight
        );

        _tileWidth = _playField.width / _gridDimension;
        _tileHeight = _playField.height / _gridDimension;

    }

    public void FillEmptyTiles()
    {
        for(int col = 0; col < _gridDimension; col++)
        {
            for(int row = 0; row < _gridDimension; row++)
            {
                Tile tile = _tileMap[col, row];
                if(tile.GetTileObjType() == TileObjType.None)
                {
                    Debug.Log("Detected empty tile starting recursive algorithm. Empty tile is: " + tile.TilePos);
                    FillColumn(tile); // This will recursively fill the empty tiles.
                    break;
                }
            }
        }
    }

    void FillColumn(Tile tile)
    {
        if(tile.GetTileObjType() == TileObjType.None)
        {
            if(tile.GetTileObjType() == TileObjType.Absent)
            {
                if(tile.TilePos.y + 1 >= _gridDimension) return;
                else FillColumn(_tileMap[tile.TilePos.x, tile.TilePos.y + 1]);
            }

            Debug.Log("Recursive algorithm started tile (" + tile.TilePos + ") seems to be empty attempting to fill. ");
            for(int row = tile.TilePos.y + 1; row < _gridDimension; row++)
            {
                Tile tileAbove = _tileMap[tile.TilePos.x, row];
                if(tileAbove.GetTileObjType() != TileObjType.None && tileAbove.GetTileObjType() != TileObjType.Absent) // Change with tile.CanFall()
                {
                    TileObjType type = tileAbove.GetTileObjType();
                    tileAbove.SetTile(TileObjType.None);
                    StartCoroutine(FallToPosition(type, tileAbove.transform.position, tile));
                    // tile.SetTile(tileAbove.GetTileObjType());
                    break;
                }
            }
        }

        if(tile.TilePos.y + 1 >= _gridDimension) return;
        else FillColumn(_tileMap[tile.TilePos.x, tile.TilePos.y + 1]);

    }

    IEnumerator FallToPosition(TileObjType tileObjType, Vector2 startPosition, Tile tile)
    {
        GameObject tileObject = Instantiate(_tileObjPrefabMap[tileObjType], startPosition, Quaternion.identity);
        tileObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Vector2 targetPosition = tile.transform.position;

        float duration = 0.2f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            tileObject.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        tileObject.transform.position = targetPosition; // Ensure it ends exactly at the target
        Destroy(tileObject);
        tile.SetTile(tileObjType);
    }

    private Vector2 GridToWorldPosition(int col, int row)
    {
        float x = _playField.xMin + col * _tileWidth - _tileWidth / 2;
        float y = _playField.yMin + row * _tileHeight - _tileHeight / 2;

        return new Vector2(x, y);
    }

    public void OnTapInput(Vector2 touchScreenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchScreenPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if(hit && hit.collider.CompareTag("Tile"))
        {
            int clickedTileId = TilePosCoordToInt(hit.collider.transform.GetComponent<Tile>().TilePos);
            _grid.ClickTile(clickedTileId);
        }
    }


    public Tile GetTile(Vector2Int tilePos)
    {
        return _tileMap[tilePos.x, tilePos.y];
    }

    public Tile GetTile(int tileNum)
    {
        return _tileMap[tileNum / _gridDimension, tileNum % _gridDimension];
    }

    public int TilePosCoordToInt(Vector2Int pos)
    {
        return _gridDimension * pos.x + pos.y;
    }

    public void OnTileHit(int col, int row)
    {
        _tileMap[col, row].OnHit();
    }

}
