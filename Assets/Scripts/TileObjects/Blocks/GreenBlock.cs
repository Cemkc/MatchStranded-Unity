using UnityEngine;

public class GreenBlock : Block
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Green;
    }

    public override Color GetParticleColor()
    {
        return Color.green;
    }
}
