using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speed;
    private AirplanePhysics _airplanePhysics;
    private float _planeSpeed;

    private void Awake()
    {
        if (GetComponentInParent<AirplanePhysics>() != null)
        {
            _airplanePhysics = GetComponentInParent<AirplanePhysics>();
        }
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
