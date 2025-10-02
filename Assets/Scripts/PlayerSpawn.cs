using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;

    private void Start()
    {
        transform.localPosition = new Vector3(_settingsData.PilotX, _settingsData.PilotY, _settingsData.PilotZ);
    }
}
