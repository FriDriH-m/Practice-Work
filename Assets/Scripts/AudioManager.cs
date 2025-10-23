using System.IO;
using UnityEngine;
using Utils;

public class AudioManager : MonoBehaviour
{
    private AudioClip[] _audioClips;
    private AudioSource _audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _audioClips = Resources.LoadAll<AudioClip>("Audio/Music");
        _audioSource = gameObject.GetComponent<AudioSource>();

        DIContainer.Instance.Register<AudioManager>(this, isSingleton: true);        
    }
    private void Start()
    {
        _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length - 1)];
        _audioSource.loop = true;
        _audioSource.Play();
    }
    public void Stop()
    {
        _audioSource?.Stop();
    }
}
