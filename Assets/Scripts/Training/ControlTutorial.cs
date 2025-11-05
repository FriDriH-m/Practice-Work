using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ControlTutorial : MonoBehaviour, ITutorialStage
{
    [SerializeField] private Outline[] _outlines;
    [SerializeField] private GameObject _endTutorial;
    private int _passedTops = 0;
    private int _hittedTarget = 0;
    

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
        foreach(var outline  in _outlines)
        {
            outline.enabled = true;
        }
        
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
        _endTutorial.SetActive(true);
        gameObject.SetActive(false);
    }
}
