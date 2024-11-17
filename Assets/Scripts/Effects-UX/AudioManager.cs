using System.Collections.Generic;
using UnityEngine;

public enum AudioName
{
    None,
    Balloon,
    CubeCollect,
    CubeExplode,
    Duck
}


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager s_Instance;

    [SerializeField] private Sound[] _sounds;
    private Dictionary<AudioName, AudioSource> _audioClipDict;

    void Awake()
    {
        #region Singleton
        if(s_Instance != null && s_Instance != this)
        {
            Destroy(this);
        }
        else
        {
            s_Instance = this;
        }
        #endregion

        _audioClipDict = new Dictionary<AudioName, AudioSource>();

        foreach (var sound in _sounds)
        {
            sound.Init(gameObject.AddComponent<AudioSource>());

            if(!_audioClipDict.ContainsKey(sound.audioType)){
                _audioClipDict[sound.audioType] = sound.source;
            }
        }
    }

    public void Play(AudioName audioType)
    {
        _audioClipDict[audioType].Play();
    }
}
