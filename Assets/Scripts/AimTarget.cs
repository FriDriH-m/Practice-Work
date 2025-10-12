using UnityEngine;
using Utils;

public class AimTarget : MonoBehaviour
{
    private MatchManager _matchManager;
    private bool _isAffected;
    private void Start()
    {
        _isAffected = false;
        _matchManager = DIContainer.Instance.Get<MatchManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !_isAffected)
        {
            _matchManager.IncreaseAffectedTargets();
            _isAffected = true;
        }
    }
}
