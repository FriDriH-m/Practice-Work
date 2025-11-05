using UnityEngine;
using Utils;

public class Target : MonoBehaviour
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
        if (other.gameObject.CompareTag("Plane") && !_isAffected)
        {
            _matchManager.IncreaseAffectedTops();
            _isAffected = true;
            gameObject.SetActive(false);
        }
    }
}
