using UnityEngine;

[CreateAssetMenu(fileName = "TileMoveAnimation", menuName = "Scriptable Objects/Tile Animaitons")]
public class TileMoveAnimation : ScriptableObject
{
    public float blockToGoalDuration = 1.0f; 
    public AnimationCurve blockToGoalScaleCurve;
    public AnimationCurve blockToGoalMoveCurve;
}