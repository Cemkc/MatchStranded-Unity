using UnityEngine;

public interface IInputInvoker
{
    public void ConnectIput(LevelManager level);
    public void DisconnectInput(LevelManager level);
}
