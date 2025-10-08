using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Utils;

public class AirplaneGuns : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private InputActionReference _controllerTrigger;
    private XRInput _XRInput;
    private AudioSource _audioSource;
    private bool _isPlaying = false;
    private BulletObjectPool _objectPool;


    private void Awake()
    {
        _XRInput = DIContainer.Instance.Get<XRInput>();

        _objectPool = GetComponent<BulletObjectPool>();

        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();
    }

    private void Update()
    {
        if (DIContainer.Instance.Get<XRInput>().XRILeftInteraction.ActivateValue.ReadValue<float>() > 0.1f)
        {
            _objectPool.GetFromPool();
            if (_audioSource.time >= 2f)
            {
                _audioSource.time = 0;
            }
            if (_isPlaying) return;
            _audioSource.Play();
            _audioSource.time = 0.2f;
            _isPlaying = true;
        }
        else
        {
            if (!_isPlaying) return;
            _audioSource.time = 2f;
            _isPlaying = false;
        }
    }
}
