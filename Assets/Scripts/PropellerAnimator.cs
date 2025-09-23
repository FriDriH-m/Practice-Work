using UnityEngine;

public class PropellerAnimator : MonoBehaviour
{
    private AirplanePhysics _airplanePhysics;
    private Animator _animator;

    private void Awake()
    {
        _airplanePhysics = GetComponentInParent<AirplanePhysics>();
        _animator = GetComponent<Animator>();
        if (_airplanePhysics == null) Debug.LogWarning("PropellerAnimator: did not find AirplanePhysics");
    }

    private void Update()
    {
        _animator.speed = _airplanePhysics.Thrust * 10;
    }
}
