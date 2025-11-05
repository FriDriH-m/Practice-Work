using Bhaptics.SDK2;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Utils;

public class AirplaneGuns : MonoBehaviour
{
    private BhapticManager _bhapticManager;
    private XRInput _XRInput;
    private AudioSource _audioSource;
    private bool _isPlaying = false;
    private GunsShootingSystem _gunShootingSystem;
    private readonly Dictionary<string, string> handEvent = new Dictionary<string, string>()
    {
        { "LeftHand", BhapticsEvent.PLANEFIRELEFTHAND },
        { "RightHand", BhapticsEvent.PLANEFIRERIGHTHAND }
    };

    private void Awake()
    {
        _gunShootingSystem = GetComponent<GunsShootingSystem>();
        
        DIContainer.Instance.Register(_gunShootingSystem, "Player_Bullets");
        DIContainer.Instance.Register(this, isSingleton: true);
    }
    private void Start()
    {
        _XRInput = DIContainer.Instance.Get<XRInput>();
        
        _bhapticManager = DIContainer.Instance.Get<BhapticManager>();

        _audioSource = GetComponent<AudioSource>();        
        _audioSource.Stop();
        _gunShootingSystem.Init(DIContainer.Instance.Get<AirplanePhysics>("Player_Plane"));
    }    

    private void Update()
    {
        if (_gunShootingSystem.Ammo.Count == 0)
        {
            ReloadGuns();
            return;
        }
        var activeHand = DIContainer.Instance.Get<YokeRotator>("Plane_Yoke").ActiveHand;
        if (activeHand == null) return;
        if (GetTriggerValue(activeHand.tag) >= 0.1f)
        {
            _gunShootingSystem.Shoot();

            _bhapticManager.RequestStartEvent(handEvent[activeHand.tag]);

            PlayAudio();
        }
        else
        {
            StopAudio();
        }
    }
    private float GetTriggerValue(string hand)
    {
        return hand switch
        {
            "LeftHand" => _XRInput.XRILeftInteraction.ActivateValue.ReadValue<float>(),
            "RightHand" => _XRInput.XRIRightInteraction.ActivateValue.ReadValue<float>(),
            _ => 0f,
        };
    }
    private void ReloadGuns()
    {
        _gunShootingSystem.Reload();

        if (!_isPlaying) return;
        _audioSource.time = 2f;
        _isPlaying = false;
        StopAudio();
    }
    private void StopAudio()
    {
        if (!_isPlaying) return;
        _audioSource.time = 2f;
        _isPlaying = false;
    }
    private void PlayAudio()
    {
        if (_audioSource.time >= 2f)
        {
            _audioSource.time = 0;
        }
        if (_isPlaying) return;
        _audioSource.Play();
        _audioSource.time = 0.2f;
        _isPlaying = true;
    }
}
