using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}

    Editor editor;

    public InspectorView()
    {
        Add(new Button());
    }

    internal void SetInspectorView(GridBlueprint gridBlueprint)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(gridBlueprint);
        IMGUIContainer container = new IMGUIContainer(() => {editor.OnInspectorGUI();});
        Add(container);
    }
}
