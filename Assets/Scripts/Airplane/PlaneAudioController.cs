using UnityEngine;
using Utils;

public class PlaneAudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    private AirplanePhysics _airplanePhysics;
    private float _preValue = 0f;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
    }

    private void Update()
    {
        if (_audioSource == null) return;

        float target = _airplanePhysics.Thrust * 1.5f; 
        _audioSource.pitch = Mathf.Lerp(_audioSource.pitch, target, 0.05f); 
    }
}
