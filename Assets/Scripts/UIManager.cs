using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager s_Instance;

    [SerializeField] private RectTransform _goalsRectTransform;
    [SerializeField] private GameObject _goalTileUIPrefab;
    [SerializeField] private Text _movesText;
    private Dictionary<TileObjectType, GameObject> _uiObjectsDict;

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
    }

    void Start()
    {
        LevelManager.s_Instance.OnGoalCountChange += OnGoalCountChange;
        LevelManager.s_Instance.OnMovesCountChange += OnMovesCountChange;

        _uiObjectsDict = new Dictionary<TileObjectType, GameObject>();

        HorizontalLayoutGroup layoutGroup = _goalsRectTransform.transform.GetComponent<HorizontalLayoutGroup>();

        foreach (KeyValuePair<TileObjectType, TileGoalData> pair in LevelManager.s_Instance.TileGoalDict)
        {
            GameObject child = Instantiate(_goalTileUIPrefab, _goalsRectTransform.transform);
            if(!_uiObjectsDict.ContainsKey(pair.Key)) _uiObjectsDict[pair.Key] = child;
            child.GetComponentInChildren<Image>().sprite = pair.Value.tileObjectPrefab.GetComponent<SpriteRenderer>().sprite;
            child.GetComponentInChildren<Text>().text = pair.Value.goalCount.ToString();
        }

        _movesText.text = LevelManager.s_Instance.MoveCount.ToString();
    }

    void OnDisable()
    {
        LevelManager.s_Instance.OnGoalCountChange -= OnGoalCountChange;
        LevelManager.s_Instance.OnMovesCountChange -= OnMovesCountChange;
    }

    private void OnMovesCountChange(int movesCount)
    {
        int movesCountText;
        if(movesCount < 0) movesCountText = 0;
        else movesCountText = movesCount;
        _movesText.text = movesCountText.ToString();
    }

    private void OnGoalCountChange(TileObjectType type, int count)
    {
        int countText;
        if(count < 0) countText = 0;
        else countText = count;
        _uiObjectsDict[type].GetComponentInChildren<Text>().text = countText.ToString();
    }
    
    public Vector2 GoalPositionUI(TileObjectType type)
    {
        Rect worldRect = GridUtils.GetWorldSpaceRect(_uiObjectsDict[type].GetComponent<RectTransform>());
        return worldRect.center;
    }

}