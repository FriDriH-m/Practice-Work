using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game/PlayerSettings")]
public class SettingsData : ScriptableObject
{
    [SerializeField] private float _pilotX = 0f;
    [SerializeField] private float _pilotY = -0.6f;
    [SerializeField] private float _pilotZ = 0.6f;

    public float PilotX => _pilotX;
    public float PilotY => _pilotY;
    public float PilotZ => _pilotZ;

    public void SetPilotLanding(float pilotX, float pilotY, float pilotZ)
    {
        _pilotX = pilotX;
        _pilotY = pilotY;
        _pilotZ = pilotZ;
    }
}
