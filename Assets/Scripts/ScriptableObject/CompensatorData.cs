using UnityEngine;

[CreateAssetMenu(fileName = "CompensatorSettings", menuName = "Game/CompensatorSettings")]
public class CompensatorData : ScriptableObject
{
    [SerializeField] private float _pitchMultiplie = 0.2f;
    [SerializeField] private float _rollMultiplie = 0.4f;
    [SerializeField] private float _rollSmoothTime = 0.4f;
    [SerializeField] private float _pitchSmoothTime = 0.55f;
    public float PitchMultiplie => _pitchMultiplie;
    public float RollMultiplie => _rollMultiplie;
    public float RollSmoothTime => _rollSmoothTime;
    public float PitchSmoothTime => _pitchSmoothTime;


    public void SetValues(float pitchMultiplie, float rollMultiplie, float rollSmoothTime, float pitchSmoothTime)
    {
        _pitchMultiplie = pitchMultiplie;
        _rollMultiplie = rollMultiplie;
        _rollSmoothTime = rollSmoothTime;
        _pitchSmoothTime = pitchSmoothTime;
    }
}
