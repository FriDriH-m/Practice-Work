using System;
using UnityEngine;
using Utils;

public class MatchManager
{
    private int _affectedTargets;
    private int _affectedTops;

    public event Action PlayerPassedRing;
    public event Action PlayerHitTarget;

    public int AffectedTargets => _affectedTargets;
    public int AffectedTops => _affectedTops;
    public void IncreaseAffectedTops()
    {
        _affectedTops++;
        PlayerPassedRing?.Invoke();
    }
    public void IncreaseAffectedTargets()
    {
        _affectedTargets++;
        PlayerHitTarget?.Invoke();
    }
}
