using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

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
        //_XRInput = ServiceLocator.Instance.Get<XRInput>();
        //_objectPool = ServiceLocator.Instance.Get<BulletObjectPool>();
        _XRInput = new XRInput();
        _objectPool = GetComponent<BulletObjectPool>();

        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();
    }
    private void OnEnable()
    {
        _XRInput.Enable();
    }

    private void Update()
    {
        //if (_XRInput == null) Debug.Log("XRInput Нулл");
        if (_XRInput.XRILeftInteraction.ActivateValue.ReadValue<float>() > 0.1f)
        {
            //if (_objectPool == null) Debug.Log("Нулл"); 
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
