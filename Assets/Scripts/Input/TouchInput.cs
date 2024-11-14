using UnityEngine;
using UnityEngine.Events;

public class TouchInput : MonoBehaviour, IInputInvoker
{
    public UnityAction<Vector2> TouchBegan;

    public void ConnectIput(GridManager level)
    {
        TouchBegan += level.OnTapInput;
    }

    public void DisconnectInput(GridManager level)
    {
        TouchBegan -= level.OnTapInput;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                TouchBegan?.Invoke(touch.position);
            }
        }
    }

    

}
