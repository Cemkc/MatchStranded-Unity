using UnityEngine;

public enum ParticleName
{
    None,
    CubeExplode
}

[System.Serializable]
public class Particle
{
    [SerializeField] private Transform _transform;

    public ParticleName particleName;
    [SerializeField] private ParticleSystem _particleSystem;
    private ParticleSystem.MainModule _particleSettings;

    public Transform Transform { get => _transform; set => _transform = value; }

    public void Play()
    {
        Debug.Log("Trying to play the particle effect! ");
        _particleSystem.Play();
    }

    public void SetColor(Color color)
    {
        _particleSettings.startColor = color;
    }

}