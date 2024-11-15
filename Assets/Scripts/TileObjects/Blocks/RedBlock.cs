using UnityEngine;

public class RedBlock : Block
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Red;
    }

    public override Color GetParticleColor()
    {
        return Color.red;
    }
}
