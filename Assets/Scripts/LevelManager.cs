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

    [SerializeField] private int _moveCount;

    [SerializeField] private GoalAttribution[] _goalAttributions;
    [SerializeField] private Dictionary<TileObjectType, int> _goalDict;

    // public float animationDuration = 1.0f; 
    // public AnimationCurve scaleCurve;
    // public AnimationCurve moveCurve; 

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

        foreach (GoalAttribution goalAttribution in _goalAttributions)
        {
            if(!_goalDict.ContainsKey(goalAttribution.tileObject)){
                _goalDict[goalAttribution.tileObject] = goalAttribution.goalNumber;
            }
        }
    }


    public bool CountsTowardsGoal(TileObject tileObject)
    {
        if(!(_goalDict.ContainsKey(tileObject.Type) && _goalDict[tileObject.Type] > 0)) return false;
        else
        {
            MoveTowardsGoal(tileObject, Vector3.zero, GameManager.Settings.MoveToGoalAnimation);
            return true;
        }
    }

    private IEnumerator MoveTowardsGoal(TileObject tileObject, Vector3 targetPosition, TileMoveAnimation animation)
    {
        yield return StartCoroutine(GridManager.MoveTileObjectToPosition(tileObject, targetPosition, animation));
        TileObjectGenerator.s_Instance.ReturnTileObject(tileObject);
    }

}