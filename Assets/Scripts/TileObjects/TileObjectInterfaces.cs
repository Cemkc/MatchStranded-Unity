
public interface IHitableTileobject
{
    public void OnHit(int damage);
}

public interface IMatchSensitive
{
    public void OnMatchHit();
}

public interface IAudible
{
    public AudioName GetAudioName();
}

public interface IParticleEmitting
{
    public ParticleName GetParticleName();
    public UnityEngine.Color GetParticleColor();
}
