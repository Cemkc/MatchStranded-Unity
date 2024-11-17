using UnityEngine;

[System.Serializable]
public class Sound 
{
    public AudioName audioType;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public void Init(AudioSource audioSource)
    {
        source = audioSource;

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

}
