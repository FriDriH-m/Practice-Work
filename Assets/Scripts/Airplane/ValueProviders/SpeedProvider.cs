using Interfaces;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Utils;

public class SpeedProvider : MonoBehaviour, IIndicatorProvider
{
    private AirplanePhysics _airplanePhysics;
    private void Start()
    {
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
    }
    public float GetValue()
    {
        return _airplanePhysics.LocalVelocity.magnitude * 3.6f;
    }
}
