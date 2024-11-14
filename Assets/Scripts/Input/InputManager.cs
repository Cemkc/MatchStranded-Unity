using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager s_Instance;
    private IInputInvoker _inputInvoker;

    void Awake()
    {
        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this); 
        }
        else 
        { 
            s_Instance = this; 
        }

        IInputInvoker[] inputInvokers = transform.GetComponents<IInputInvoker>();
        _inputInvoker = inputInvokers[0];
    }

    public void ConnectInput(GridManager levelManager)
    {
        _inputInvoker.ConnectIput(levelManager);
    }

    void OnDisable()
    {
        _inputInvoker.DisconnectInput(GridManager.s_Instance);
    }
}
