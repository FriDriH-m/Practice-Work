using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PilotLandingSettings : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Scrollbar _yScrollbar;
    [SerializeField] private SettingsData _playerSettings;
    //[SerializeField] private Scrollbar _xScrollbar;
    private void Start()
    {
        _yScrollbar.value = Mathf.InverseLerp(-1, 0f, _playerSettings.PilotY);
    }

    private void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, _zStart + Mathf.Lerp(-0.3f, 0.3f, _xScrollbar.value));
    }
}
