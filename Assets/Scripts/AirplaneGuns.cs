using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class AirplaneGuns : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    private XRInput _XRInput;
    private AudioSource _audioSource;
    private bool _isPlaying = false;
    private ObjectPool _objectPool;


    private void Awake()
    {
        _XRInput = new XRInput();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();
        _objectPool = GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        _XRInput.Enable();
    }
    private void OnDisable()
    {
        _XRInput.Disable();
    }

    private void Update()
    {
        if (_XRInput.XRILeftInteraction.ActivateValue.ReadValue<float>() > 0.1f)
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
