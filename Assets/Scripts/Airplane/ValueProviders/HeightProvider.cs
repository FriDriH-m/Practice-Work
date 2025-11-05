using Interfaces;
using UnityEngine;
using Utils;

public class HeightProvider : MonoBehaviour, IIndicatorProvider
{
    private AirplanePhysics _airplanePhysics;
    private void Start()
    {
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
    }
    public float GetValue()
    {
        return _airplanePhysics.transform.position.y;
    }

}
