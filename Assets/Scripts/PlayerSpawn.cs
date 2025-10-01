using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;

    private void Start()
    {
        transform.localPosition = new Vector3(0, _settingsData.PilotHeight, _settingsData.PilotWidth);
    }
}
