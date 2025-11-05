using UnityEngine;
using Utils;

public class PropellerAnimator : MonoBehaviour
{
    private AirplanePhysics _airplanePhysics;
    private Animator _animator;

    private void Start()
    {
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
        _animator = GetComponent<Animator>();
        if (_airplanePhysics == null) Debug.LogWarning("PropellerAnimator: did not find AirplanePhysics");
    }

    private void Update()
    {
        _animator.speed = _airplanePhysics.Thrust * 10;
    }
}
