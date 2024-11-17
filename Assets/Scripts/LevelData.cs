using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Design/Level Data")]
public class LevelData : ScriptableObject
{
    public int rows;
    public int columns;
    public TileObjectType[] gridData; // Flat array for grid tiles
    public List<TileObjectType>[] tilePools; // List of tile types for each column
}