using TMPro;
using UnityEngine;
using Utils;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speed;
    private AirplanePhysics _airplanePhysics;
    private float _planeSpeed;

    private void Start()
    {
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
    }

    private void Update()
    {
        if (_airplanePhysics == null)
        {
            Debug.LogWarning("UI: _airpanePhysics is null");
        }
        _planeSpeed = _airplanePhysics.LocalVelocity.magnitude * 3.6f;
        _planeSpeed = (int)_planeSpeed;
        _speed.text = "Speed: " + _planeSpeed;
    }

    public void SetPlaneSpeed(float speed)
    {
        _planeSpeed = speed;
    }
}
