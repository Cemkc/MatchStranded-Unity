using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    private struct GoalAttribution
    {
        public TileObjectType tileObject;
        public int goalNumber;
    }

    public static LevelManager s_Instance;

    [SerializeField] private GameSettings _settings;

    [SerializeField] private RectTransform _goalsRectTransform;
    [SerializeField] private Rect _goalsRect;
    [SerializeField] private int _moveCount;
    [SerializeField] private Text _movesText;

    [SerializeField] private GoalAttribution[] _goalAttributions;
    [SerializeField] private Dictionary<TileObjectType, int> _goalCountDict;
    [SerializeField] private Dictionary<TileObjectType, TileObject> _goalObjectDict;


    public static GameSettings Settings { get => s_Instance._settings; }

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

        _goalCountDict = new Dictionary<TileObjectType, int>();
        _goalObjectDict = new Dictionary<TileObjectType, TileObject>();
        _movesText.text = _moveCount.ToString();

        foreach (GoalAttribution goalAttribution in _goalAttributions)
        {
            if(!_goalCountDict.ContainsKey(goalAttribution.tileObject)){
                _goalCountDict[goalAttribution.tileObject] = goalAttribution.goalNumber;
            }
        }
    }

    void Start()
    {
        _goalsRect = GridUtils.GetWorldSpaceRect(_goalsRectTransform);

        float tileWidth;
        float tileHeight;

        tileWidth = _goalsRect.width / _goalCountDict.Keys.Count;
        tileHeight = tileWidth;

        Vector2 startPosition = new Vector2(
            _goalsRect.x + tileWidth / 2,
            _goalsRect.y + _goalsRect.height / 2
        );


        int index = 0;
        foreach (KeyValuePair<TileObjectType, int> pair in _goalCountDict)
        {
            TileObject tileObject = TileObjectGenerator.s_Instance.GetTileObject(pair.Key);
            if(!_goalObjectDict.ContainsKey(pair.Key)) _goalObjectDict[pair.Key] = tileObject; 
            Vector2 tilePosition = new Vector3(
                startPosition.x + index * tileWidth,
                startPosition.y,
                0.0f
            );

            tileObject.transform.position = tilePosition;

            index++;
        }
    }


    public bool CountsTowardsGoal(TileObject tileObject)
    {
        if(_goalCountDict.ContainsKey(tileObject.Type) && _goalCountDict[tileObject.Type] > 0){
            Vector3 pos = tileObject.transform.position;
            pos.z--;
            tileObject.transform.position = pos;
            TileObject goalObject = _goalObjectDict[tileObject.Type];
            StartCoroutine(MoveTowardsGoal(tileObject, goalObject.transform.position, _settings.MoveToGoalAnimation));
            return true;
        }
        else return false;
    }

    private IEnumerator MoveTowardsGoal(TileObject tileObject, Vector3 targetPosition, TileMoveAnimation animation)
    {
        yield return StartCoroutine(GridUtils.MoveTileObjectToPosition(tileObject, targetPosition, animation));
        TileObjectGenerator.s_Instance.ReturnTileObject(tileObject);
    }

    public void DecreaseMoveCount(int number)
    {
        _moveCount -= number;
        _movesText.text = _moveCount.ToString();
    }

}