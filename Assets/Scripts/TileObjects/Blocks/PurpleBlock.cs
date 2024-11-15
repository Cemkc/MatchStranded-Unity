using UnityEngine;

public class PurpleBlock : Block
{
    public override void OnAwakeFunction()
    {
        base.OnAwakeFunction();
        _type = TileObjectType.Purple;
    }

    public override Color GetParticleColor()
    {
        return Color.red + Color.blue;
    }
}
