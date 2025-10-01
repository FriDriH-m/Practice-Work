using UnityEngine;

public class PlaneAudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    private AirplanePhysics _airplanePhysics;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _airplanePhysics = GetComponent<AirplanePhysics>();
    }

    private void Update()
    {
        if (_audioSource != null)
        {
            _audioSource.pitch = _airplanePhysics.Thrust * 1.5f;
        }
    }
}
