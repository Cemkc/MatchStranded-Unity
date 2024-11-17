using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

public class LevelEditorView : GraphView
{
    public new class UxmlFactory : UxmlFactory<LevelEditorView, GraphView.UxmlTraits> {}

    GridView gridView;

    public LevelEditorView(){

        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LevelEditorWindow.uss");
        styleSheets.Add(styleSheet);

    }

    internal void PopulateView(GridBlueprint gridBlueprint)
    {
        DeleteElements(graphElements);

        if(gridBlueprint != null && gridBlueprint.OcccupiedPositions.Length == gridBlueprint.Dimension * gridBlueprint.Dimension){
            Debug.Log("Creating from the beginning");
            gridView = new GridView(gridBlueprint);
            AddElement(gridView);
        }
        else{
            Debug.Log("Asset is not suitable for grid view. ");
        }
    }
}
