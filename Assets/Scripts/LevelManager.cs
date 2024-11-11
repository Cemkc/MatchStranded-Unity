using System.Collections.Generic;
using Flap;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager s_Instance;

    [SerializeField] private int _playfieldDimension;
    [SerializeField] private List<GridBlueprint> _gridBlueprints;
    private List<Grid> _grids;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private List<GameObject> _tileObjects;
    private Dictionary<int, Tile> _tileMap;

    private Rect _playField;

    public int PlayfieldDimension { get => _playfieldDimension; }
    public Dictionary<int, Tile> TileMap { get => _tileMap; }
    public List<GameObject> TileObjects { get => _tileObjects; }

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
        
        _tileMap = new Dictionary<int, Tile>();

    }

    void Start()
    {
        GenerateBaseGrid();
    }

    void Update()
    {
        
    }

    void GenerateBaseGrid()
    {
        SetPlayField();

        float tileWidth = _playField.width / _playfieldDimension;
        float tileHeight = _playField.height / _playfieldDimension;

        Vector2 startPosition = new Vector2(
            _playField.xMin + tileWidth / 2,
            _playField.yMin + tileHeight / 2
        );

        int tileIndex = 0;
        for (int row = 0; row < _playfieldDimension; row++)
        {
            for (int col = 0; col < _playfieldDimension; col++)
            {
                GameObject tile = Instantiate(_tilePrefab, transform);
    
                tile.transform.localScale = new Vector3(tileWidth, tileHeight, 1);
    
                Vector2 tilePosition = new Vector2(
                    startPosition.x + col * tileWidth,
                    startPosition.y + row * tileHeight
                );

                tile.transform.position = tilePosition;
                tile.name = "(" + row + ", " + col + ")" + " - " + tileIndex;

                _tileMap[tileIndex] = tile.GetComponent<Tile>();

                tile.GetComponent<Tile>().Init(_tileObjects);

                tileIndex++;
            }
        }

        Grid grid = new Grid(_gridBlueprints[0]);
        grid.RandomFillTiles();
        
        grid.ClickTile(3);
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
    }
}
