using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;

    private void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _settingsData.PilotY - 0.4f, transform.localPosition.z);
    }
}
