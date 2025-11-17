using UnityEngine;
using Utils;

public class PropellerAnimator : MonoBehaviour
{
    [SerializeField] private float smooth = 2f; 

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
        float target = _airplanePhysics.Thrust * 10f;
        _animator.speed = Mathf.Lerp(_animator.speed, target, smooth * Time.deltaTime);
    }
}
