using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ControlTutorial : MonoBehaviour, ITutorialStage
{
    [SerializeField] private Outline _outline;
    private int _passedTops = 0;
    private int _hittedTarget = 0;
    private bool _isComplete = false;
    

    private void PassedRing()
    {
        _passedTops++;
    }
    private void HitTarget()
    {
        _hittedTarget++;
    }
    public void ActivateStage()
    {
        gameObject.SetActive(true);
        var manager = DIContainer.Instance.Get<MatchManager>();
        manager.PlayerPassedRing += PassedRing;
        manager.PlayerHitTarget += HitTarget;
    }

    public bool CheckProgress()
    {
        if (_passedTops >= 1 && _hittedTarget >= 1)
        {
            return true;
        }
        return false;
    }

    public void CompleteStage()
    {
        _outline.enabled = false;
        gameObject.SetActive(false);
    }
}
