using UnityEngine;
using Utils;

public class AirplaneControl : MonoBehaviour
{
    [SerializeField] private Transform _yoke;
    [SerializeField] private AirplanePhysics _airplanePhysics;

    private void Awake()
    {
        DIContainer.Instance.Register<AirplanePhysics>(_airplanePhysics, "Player_Plane", true);
        DIContainer.Instance.Register<Transform>(transform, "Player", true);
    }
    private void Start()
    {
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
    }

    private void FixedUpdate()
    {
        var targetInput = _yoke.localEulerAngles;
        if (Mathf.Abs(_yoke.localEulerAngles.x) < 1) targetInput.x = 0;
        if (Mathf.Abs(_yoke.localEulerAngles.z) < 1) targetInput.z = 0;
        _airplanePhysics.SetSteeringInput(targetInput);
    }
}
