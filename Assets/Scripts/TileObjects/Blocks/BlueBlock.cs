using UnityEngine;

public class BlueBlock : Block
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Blue;
    }

    public override Color GetParticleColor()
    {
        return Color.blue;
    }
}
