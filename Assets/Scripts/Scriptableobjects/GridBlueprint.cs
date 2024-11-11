using UnityEngine;

[CreateAssetMenu(fileName = "Grid Blueprint", menuName = "Scriptable Objects / Grid Blueprint")]
public class GridBlueprint : ScriptableObject
{

    [SerializeField] private int _playFieldDimension;
    [SerializeField] private int[] _occcupiedPositions;
    [SerializeField] private GameObject[] _blockPrefabs;

    public int PlayFieldDimension { get => _playFieldDimension; }
    public int[] OcccupiedPositions { get => _occcupiedPositions; }
    public GameObject[] BlockPrefabs { get => _blockPrefabs; }
}
