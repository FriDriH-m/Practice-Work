using System;
using UnityEngine;
using Utils;

public class MatchManager
{
    private int _affectedTargets;
    private int _affectedTops;

    public event Action PlayerWon;

    public int AffectedTargets => _affectedTargets;
    public int AffectedTops => _affectedTops;
    public void IncreaseAffectedTops()
    {
        _affectedTops++;
        if (AffectedTargets == 3 && AffectedTops == 3)
        {
            PlayerWon?.Invoke();
        }
    }
    public void IncreaseAffectedTargets()
    {
        _affectedTargets++;
        if (AffectedTargets == 3 && AffectedTops == 3)
        {
            PlayerWon?.Invoke();
        }
    }
}
