using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils{

    private static GridManager _gridManager = GridManager.s_Instance;

    #region Utility functions

    public static IEnumerator MoveTileObjectToPosition(TileObject tileObject, Vector3 targetPosition, TileMoveAnimation animation)
    {
        float elapsedTime = 0f;

        Vector3 startPosition = tileObject.transform.position;

        Vector3 initialScale = tileObject.transform.localScale;

        while (elapsedTime < animation.blockToGoalDuration)
        {
            // Calculate normalized time [0, 1]
            float normalizedTime = elapsedTime / animation.blockToGoalDuration;

            // Interpolate position
            float positionFactor = animation.blockToGoalMoveCurve.Evaluate(normalizedTime);
            tileObject.transform.position = Vector3.Lerp(startPosition, targetPosition, positionFactor);

            // Interpolate scale
            float scaleFactor = animation.blockToGoalScaleCurve.Evaluate(normalizedTime);
            tileObject.transform.localScale = initialScale * scaleFactor;

            // Increment time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the final position and scale are set
        tileObject.transform.position = targetPosition;
        tileObject.transform.localScale = initialScale * animation.blockToGoalScaleCurve.Evaluate(1f);
    }

    public static Vector2 GridToWorldPosition(int col, int row)
    {
        float x = _gridManager.PlayFieldRect.xMin + _gridManager.TileWidth / 2 + col * _gridManager.TileWidth;
        float y = _gridManager.PlayFieldRect.yMin + _gridManager.TileHeight / 2 + row * _gridManager.TileHeight;

        return new Vector2(x, y);
    }

    public static void GetConnectedTiles(int tile, ref List<int> connectedTiles, ref List<int> hittableTilesOnEdge, int previousTile = -1)
    {
        List<int> adjacentTiles = GetAdjacentTiles(tile);

        if(previousTile == -1)
        {
            connectedTiles.Add(tile);
        }
        
        foreach (int adjacentTile in adjacentTiles)
        {
            if(adjacentTile == previousTile)
            {
                continue;
            }
            TileObjectType selfType = _gridManager.GetTile(tile).GetTileType();
            TileObjectType adjacentType = _gridManager.GetTile(adjacentTile).GetTileType();
            if(selfType == adjacentType && adjacentType != TileObjectType.Absent)
            {
                if(!connectedTiles.Contains(adjacentTile))
                {
                    connectedTiles.Add(adjacentTile);
                    GetConnectedTiles(adjacentTile, ref connectedTiles, ref hittableTilesOnEdge, tile);
                }
            }
            else if(_gridManager.GetTile(adjacentTile).GetTileCategory().HasFlag(TileObjectCategory.HitableTileObject))
            {
                hittableTilesOnEdge.Add(adjacentTile);
            }
        }

        return;
    }

    public static List<int> GetAdjacentTiles(int tile)
    {
        List<int> adjacentTiles = new List<int>();

        // Calculate row and column of the given tile
        int row = tile % GridManager.GridDimension;
        int col = tile / GridManager.GridDimension;

        // Check above (row + 1)
        if (row + 1 < GridManager.GridDimension && _gridManager.GetTile(tile + 1).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile + 1);
        }

        // Check below (row - 1)
        if (row - 1 >= 0 && _gridManager.GetTile(tile -1).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile -1);
        }

        // Check right (col + 1)
        if (col + 1 < GridManager.GridDimension && _gridManager.GetTile(tile + GridManager.GridDimension).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile + GridManager.GridDimension);
        }

        // Check left (col - 1)
        if (col - 1 >= 0 && _gridManager.GetTile(tile - GridManager.GridDimension).GetTileType() != TileObjectType.Absent)
        {
            adjacentTiles.Add(tile - GridManager.GridDimension);
        }

        return adjacentTiles;
    }

    public static Rect GetWorldSpaceRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // Bottom-left corner
        Vector3 bottomLeft = corners[0];
        // Top-right corner
        Vector3 topRight = corners[2];

        float width = Mathf.Abs(topRight.x - bottomLeft.x);
        float height = Mathf.Abs(topRight.y - bottomLeft.y);

        return new Rect(bottomLeft.x, bottomLeft.y, width, height);
    }

    public static int TilePosToId(Vector2Int pos)
    {
        return GridManager.GridDimension * pos.x + pos.y;
    }

    public static Vector2Int TileIdToPos(int id)
    {
        int col = id / GridManager.GridDimension;
        int row = id % GridManager.GridDimension;
        return new Vector2Int(col, row);
    }

    #endregion

}