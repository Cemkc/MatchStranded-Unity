using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private int _moveCount;

    [SerializeField] private GoalAttribution[] _goalAttributions;
    [SerializeField] private Dictionary<TileObjectType, int> _goalDict;


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

        _goalDict = new Dictionary<TileObjectType, int>();

        foreach (GoalAttribution goalAttribution in _goalAttributions)
        {
            if(!_goalDict.ContainsKey(goalAttribution.tileObject)){
                _goalDict[goalAttribution.tileObject] = goalAttribution.goalNumber;
            }
        }
    }


    public bool CountsTowardsGoal(TileObject tileObject)
    {
        if(_goalDict.ContainsKey(tileObject.Type) && _goalDict[tileObject.Type] > 0){
            Vector3 pos = tileObject.transform.position;
            pos.z--;
            tileObject.transform.position = pos;
            Debug.Log("Starting towards goal coroutine.");
            StartCoroutine(MoveTowardsGoal(tileObject, Vector3.zero, _settings.MoveToGoalAnimation));
            return true;
        }
        else return false;
    }

    private IEnumerator MoveTowardsGoal(TileObject tileObject, Vector3 targetPosition, TileMoveAnimation animation)
    {
        Debug.Log("Started towards goal coroutine");
        yield return StartCoroutine(GridManager.MoveTileObjectToPosition(tileObject, targetPosition, animation));
        TileObjectGenerator.s_Instance.ReturnTileObject(tileObject);
        Debug.Log("Ended towards goal coroutine and returned object to generator");
    }

}