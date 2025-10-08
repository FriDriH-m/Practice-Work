using UnityEngine;
using Utils;

public class PlaneAudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    private AirplanePhysics _airplanePhysics;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
    }

    private void Update()
    {
        if (_audioSource != null)
        {
            _audioSource.pitch = _airplanePhysics.Thrust * 1.5f;
        }
    }
}
