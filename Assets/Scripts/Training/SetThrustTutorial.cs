using UnityEngine;
using Interfaces;
public class SetThrustTutorial : MonoBehaviour, ITutorialStage
{
    [SerializeField] private AirplanePhysics _airplanePhysics;
    [SerializeField] private Outline _outline;
    public void ActivateStage()
    {
        if (_airplanePhysics == null) Debug.LogWarning("AirplanePhysics reference is not set in SetThrustTutorial.");
        gameObject.SetActive(true);
        _outline.enabled = true;
    }
    public bool CheckProgress()
    {
        if (_airplanePhysics.Thrust > 0.9f) return true;
        else return false;
    }

    public void CompleteStage()
    {
        _outline.enabled = false;
        gameObject.SetActive(false);
    }
}
