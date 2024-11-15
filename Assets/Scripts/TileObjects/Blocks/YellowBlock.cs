
using UnityEngine;

public class YellowBlock : Block
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Yellow;
    }

    public override Color GetParticleColor()
    {
        return Color.yellow;
    }

}
