using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestCompensator : MonoBehaviour
{
    [SerializeField] private CompensatorData compensatorData;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Scrollbar pitchMultBar;
    [SerializeField] private Scrollbar rollMultBar;
    [SerializeField] private Scrollbar pitchSmoothBar;
    [SerializeField] private Scrollbar rollSmoothBar;
    private void Start()
    {
        pitchMultBar.value = compensatorData.PitchMultiplie;
        rollMultBar.value = compensatorData.RollMultiplie;
        rollSmoothBar.value = compensatorData.RollSmoothTime;
        pitchSmoothBar.value = compensatorData.PitchSmoothTime;
    }
    private void Update()
    {
        text.text = $"Roll Multiplie: {compensatorData.RollMultiplie} \nPitch Multiolie: {compensatorData.PitchMultiplie} \nRoll Smoothtime: {compensatorData.RollSmoothTime} \nPitch SmoothTime: {compensatorData.PitchSmoothTime}";
    }
    public void SaveValues()
    {
        compensatorData.SetValues(pitchMultBar.value, rollMultBar.value, rollSmoothBar.value, pitchSmoothBar.value);
    }
}
