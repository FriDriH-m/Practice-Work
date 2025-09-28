using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game/PlayerSettings")]
public class SettingsData : ScriptableObject
{
    [SerializeField] private float _pilotHeight = -0.6f;
    [SerializeField] private float _pilotWidth = 0.6f;

    public float PilotHeight => _pilotHeight;
    public float PilotWidth => _pilotWidth;

    public void SetPilotLanding(float pilotHeight, float pilotWidth)
    {
        _pilotHeight = pilotHeight;
        _pilotWidth = pilotWidth;
    }
}
