using UnityEngine;

[CreateAssetMenu(fileName = "LevelGoals", menuName = "Scriptable Objects/Level/Level Goals")]
public class LevelGoals : ScriptableObject
{
    [SerializeField] private int _moveCount;
    [SerializeField] private TileGoalData[] _tileGoalAttribution;

    public int MoveCount { get => _moveCount; }
    public TileGoalData[] TileGoalAttribution { get => _tileGoalAttribution; }
}
