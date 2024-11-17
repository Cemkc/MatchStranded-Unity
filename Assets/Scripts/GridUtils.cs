using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUtils{

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

    #endregion

}