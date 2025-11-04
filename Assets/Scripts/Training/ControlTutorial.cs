using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ControlTutorial : MonoBehaviour, ITutorialStage
{
    [SerializeField] private Outline _outline;
    private bool _isComplete = false;

    private void PassedRing()
    {
        _isComplete = true;
    }
    public void ActivateStage()
    {
        gameObject.SetActive(true);
        DIContainer.Instance.Get<MatchManager>().PlayerPassedRing += PassedRing;
    }

    public bool CheckProgress()
    {
        return _isComplete;
    }

    public void CompleteStage()
    {
        _outline.enabled = false;
        gameObject.SetActive(false);
    }
}
