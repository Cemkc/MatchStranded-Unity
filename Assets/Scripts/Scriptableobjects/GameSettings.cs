using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private TileMoveAnimation _fallAnimation;
    [SerializeField] private TileMoveAnimation _moveToGoalAnimation;

    public TileMoveAnimation FallAnimation { get => _fallAnimation; }
    public TileMoveAnimation MoveToGoalAnimation { get => _moveToGoalAnimation; }
}
