using UnityEngine;

public interface IInputInvoker
{
    public void ConnectIput(GridManager level);
    public void DisconnectInput(GridManager level);
}
