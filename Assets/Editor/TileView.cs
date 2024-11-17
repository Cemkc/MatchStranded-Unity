using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TileView : VisualElement
{
    private GridView _gridView;

    private int _tileId;
    private Vector2 _tilePos;
    private TileObjectType _tileObjectType;
    private Color _initialColor;

    private Label _tileObjectTypeName;

    public int TileId { get => _tileId; }

    public TileView(GridView gridView, int col, int row, int dimension)
    {   
        _gridView = gridView;
        _tilePos.x = col;
        _tilePos.y = row;
        _tileId = col * dimension + row;

        SetTileViewStyle();

        // Highlight the tile when the mouse enters
        RegisterCallback<MouseEnterEvent>((e) =>
        {
            style.backgroundColor = new Color(0.875f, 0.882f, 0.89f, 1f);
        });

        RegisterCallback<MouseLeaveEvent>((e) =>
        {
            style.backgroundColor = _initialColor; 
        });

        // Handle left-click (MouseDown)
        RegisterCallback<MouseDownEvent>((e) =>
        {
            if (e.button == (int)MouseButton.LeftMouse)
            {
                Debug.Log($"Left click on tile at col {_tilePos.x}, row {_tilePos.y}");
            }

            if (e.button == (int)MouseButton.RightMouse)
            {
                Debug.Log($"Right click on tile at col {_tilePos.x}, row {_tilePos.y}");
                ShowDropdownMenu(e.mousePosition);
            }

            e.StopPropagation(); 

        });
    }

    private void ShowDropdownMenu(Vector2 position)
    {
        var dropdownMenu = new DropdownMenu();

        foreach (TileObjectType state in System.Enum.GetValues(typeof(TileObjectType)))
        {
            dropdownMenu.AppendAction(state.ToString(), (a) =>
            {
                SetTileObjectType(state);
            });
        }

        this.AddManipulator(new ContextualMenuManipulator(evt =>
        {
            foreach (TileObjectType state in System.Enum.GetValues(typeof(TileObjectType)))
            {
                evt.menu.AppendAction(state.ToString(), (a) =>
                {
                    SetTileObjectType(state);
                });
            }
        }));

    }

    public void SetTileObjectType(TileObjectType type)
    {
        _tileObjectType = type;
        _tileObjectTypeName.text = _tileObjectType.ToString();
        Debug.Log("Setting the asset");
        _gridView.SetAssetTile(_tileId, type);
    }

    private void SetTileViewStyle()
    {
        _initialColor = new Color(0.549f, 0.549f, 0.549f, 1f);
        style.backgroundColor = _initialColor;

        Color borderColor = new Color(0f, 0.384f, 0.671f, 1f);
        style.borderTopColor = borderColor;
        style.borderBottomColor = borderColor;
        style.borderLeftColor = borderColor;
        style.borderRightColor = borderColor;

        style.borderTopWidth = 2;
        style.borderBottomWidth = 2;
        style.borderLeftWidth = 2;
        style.borderRightWidth = 2;

        style.borderTopLeftRadius = 10;
        style.borderTopRightRadius = 10;
        style.borderBottomLeftRadius = 10;
        style.borderBottomRightRadius = 10;

        // Ensure the container fills the GraphElement
        style.flexGrow = 1;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);

        AddLabel();
    }

    private void AddLabel()
    {
        _tileObjectTypeName = new Label("Label");
        Add(_tileObjectTypeName);
    } 

}