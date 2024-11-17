using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphTileView : GraphElement
{
    public GraphTileView()
    {
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Column;
        container.style.alignItems = Align.Center;
        container.style.paddingTop = 10;
        container.style.paddingBottom = 10;
        container.style.paddingLeft = 10;
        container.style.paddingRight = 10;
        container.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f); // Semi-transparent gray
        container.style.borderTopLeftRadius = 5;
        container.style.borderTopRightRadius = 5;
        container.style.borderBottomLeftRadius = 5;
        container.style.borderBottomRightRadius = 5;

        // Add an image
        var image = new Image();
        image.image = EditorGUIUtility.IconContent("UnityEditor.SceneHierarchyWindow").image; // Example image
        image.style.width = 100;
        image.style.height = 100;
        container.Add(image);

        // Add a dropdown
        List<string> options = new List<string> { "Option 1", "Option 2", "Option 3" };
        var dropdown = new DropdownField(options, 0);
        dropdown.label = "Select Option:";
        dropdown.style.marginTop = 10;
        dropdown.style.width = 120;
        container.Add(dropdown);

        // Add the container to the main view
        Add(container);

        // Set position and size
        this.SetPosition(new Rect(100, 100, 140, 200)); // Default position and size

    }
}