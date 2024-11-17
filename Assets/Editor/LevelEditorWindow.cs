using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LevelEditorWindow : EditorWindow
{

    private GridBlueprint _gridBlueprint;
    private LevelEditorView _levelEditorView;
    private InspectorView _inspectorView;

    [MenuItem("Tools/LevelEditorWindow")]
    public static void OpenWindow()
    {
        LevelEditorWindow wnd = GetWindow<LevelEditorWindow>();
        wnd.titleContent = new GUIContent("LevelEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/LevelEditorWindow.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LevelEditorWindow.uss");
        root.styleSheets.Add(styleSheet);

        _levelEditorView = root.Q<LevelEditorView>();
        _inspectorView = root.Q<InspectorView>();
        
    }

    private void OnSelectionChange()
    {
        if(_gridBlueprint != null) _gridBlueprint.OnDraw -= GridBlueprintDrawCallback;
        _gridBlueprint = Selection.activeObject as GridBlueprint;
        Debug.Log("Seleciton has changed");

        if(_gridBlueprint){
            _gridBlueprint.OnDraw += GridBlueprintDrawCallback;
            _levelEditorView.PopulateView(_gridBlueprint);
            _inspectorView.SetInspectorView(_gridBlueprint);
        }
    }

    private void GridBlueprintDrawCallback(GridBlueprint gridBlueprint)
    {
        if(gridBlueprint == _gridBlueprint){
            Debug.Log("I have been called I must answer.");
            _levelEditorView.PopulateView(gridBlueprint);
        }
        // inspectorView.SetInspectorView(gridBlueprint);
    }

    private void OnTileObjectSetCallback(int tileId, TileObjectType type)
    {
        Debug.Log("The callback has been called.");
        _gridBlueprint.OcccupiedPositions[tileId] = type;
    }

}