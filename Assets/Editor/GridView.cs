using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GridView : GraphElement
{
    private Rect _position;
    private int _dimension = 8;
    private Dictionary<int, TileView> _tileViewDict;
    private GridBlueprint _gridBlueprint;

    public GridView(GridBlueprint gridBlueprint)
    {
        _tileViewDict = new Dictionary<int, TileView>();

        _gridBlueprint = gridBlueprint;
        _dimension = gridBlueprint.Dimension;

        CreateTiles();

        _position = new Rect(0, 0, 500, 500);
        SetPosition(_position);


        capabilities |= Capabilities.Movable;
        this.AddManipulator(new Dragger());
    }

    private void CreateTiles()
    {
        for (int row = _dimension - 1; row >= 0; row--)
        {
            var column = new VisualElement();
            column.style.flexDirection = FlexDirection.Row;
            column.style.flexGrow = 1;
            column.style.width = Length.Percent(100);
            column.style.height = Length.Percent(100);
            Add(column);
            for (int col = 0; col < _dimension; col++)
            {
                var tile = new TileView(this, col, row, _dimension);
                tile.style.flexDirection = FlexDirection.Column;
                column.Add(tile);
                if(!_tileViewDict.ContainsKey(tile.TileId)) _tileViewDict[tile.TileId] = tile;
            }
        }

        foreach(KeyValuePair<int, TileView> pair in _tileViewDict)
        {
            pair.Value.SetTileObjectType(_gridBlueprint.OcccupiedPositions[pair.Key]);
        }
    }

    public void SetAssetTile(int tileId, TileObjectType type)
    {
        _gridBlueprint.OcccupiedPositions[tileId] = type;
    }

}