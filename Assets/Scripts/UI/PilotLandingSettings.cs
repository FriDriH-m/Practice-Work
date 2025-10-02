using UnityEngine;
using UnityEngine.UI;

public class PilotLandingSettings : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Scrollbar _xScrollbar;
    private float _zStart;
    private void Start()
    {
        _zStart = transform.position.z;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _zStart + Mathf.Lerp(-0.3f, 0.3f, _xScrollbar.value));
    }
}
