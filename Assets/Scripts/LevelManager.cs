using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TileGoalData
{
    public GameObject tileObjectPrefab;
    public TileObjectType goalObjectType;
    public int goalCount;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager s_Instance;

    public UnityAction<int> OnMovesCountChange;
    public UnityAction<TileObjectType, int> OnGoalCountChange;

    [SerializeField] private GameSettings _settings;
    public LevelGoals levelGoals;

    private int _moveCount;

    private Dictionary<TileObjectType, TileGoalData> _tileGoalDict;
    // [SerializeField] private Dictionary<TileObjectType, int> _goalCountDict;
    // [SerializeField] private Dictionary<TileObjectType, TileObject> _goalObjectDict;

    public static GameSettings Settings { get => s_Instance._settings; }
    public int MoveCount { get => _moveCount; }
    public Dictionary<TileObjectType, TileGoalData> TileGoalDict { get => _tileGoalDict; }

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

        levelGoals = Instantiate(levelGoals);

        _moveCount = levelGoals.MoveCount;

        _tileGoalDict = new Dictionary<TileObjectType, TileGoalData>();

        foreach (TileGoalData goalAttribution in levelGoals.TileGoalAttribution)
        {
            if(!_tileGoalDict.ContainsKey(goalAttribution.goalObjectType)){
                _tileGoalDict[goalAttribution.goalObjectType] = goalAttribution;
                _tileGoalDict[goalAttribution.goalObjectType].goalCount = goalAttribution.goalCount;
                _tileGoalDict[goalAttribution.goalObjectType].tileObjectPrefab = goalAttribution.tileObjectPrefab;
            }
        }
    }

    public bool CountsTowardsGoal(TileObject tileObject)
    {
        if(_tileGoalDict.ContainsKey(tileObject.Type) && _tileGoalDict[tileObject.Type].goalCount > 0){
            Vector3 pos = tileObject.transform.position;
            pos.z--;
            tileObject.transform.position = pos;
            Vector2 uiPosition = UIManager.s_Instance.GoalPositionUI(tileObject.Type);
            StartCoroutine(MoveTowardsGoal(tileObject, uiPosition, _settings.MoveToGoalAnimation));
            return true;
        }
        else return false;
    }

    private IEnumerator MoveTowardsGoal(TileObject tileObject, Vector3 targetPosition, TileMoveAnimation animation)
    {
        yield return StartCoroutine(GridUtils.MoveTileObjectToPosition(tileObject, targetPosition, animation));
        _tileGoalDict[tileObject.Type].goalCount--;
        OnGoalCountChange?.Invoke(tileObject.Type, _tileGoalDict[tileObject.Type].goalCount);
        TileObjectGenerator.s_Instance.ReturnTileObject(tileObject);
    }

    public void DecreaseMoveCount(int number)
    {
        _moveCount -= number;
        OnMovesCountChange?.Invoke(_moveCount);
    }

}