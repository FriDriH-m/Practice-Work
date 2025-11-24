using Bhaptics.SDK2;
using Interfaces;
using UnityEngine;
using Utils;

public class FuselageComponent : PartComponent
{
    [SerializeField] private ParticleSystem _brokeParticles;
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private Vector3 _positionOffset;
    private AirplanePhysics _airplanePhysics;
    private BhapticManager _bhapticManager;
    private void Start()
    {
        _airplanePhysics = GetComponentInParent<AirplanePhysics>();
        if (_airplanePhysics == null)
        {
            Debug.LogWarning("DamageController: did not find AirplanePhysics");
        }
        _bhapticManager = DIContainer.Instance.Get<BhapticManager>();
    }
    //private void Start()
    //{
    //    var newParticles = Instantiate(_brokeParticles, transform.position + _positionOffset, transform.rotation, transform);
    //    newParticles.transform.localScale = new Vector3(20, 20, 20);
    //    newParticles.Play();
    //}
    public override void TakeDamage(int damageCount)
    {
        if (_health - damageCount > 0)
        {
            _health -= damageCount;
            _bhapticManager.RequestStartEvent(BhapticsEvent.GETDAMAGE);
        }
        else
        {
            _health = 0;
            _airplanePhysics.BrokeEngine();
            var newParticles = Instantiate(_brokeParticles, transform.position + _positionOffset, transform.rotation, transform);
            newParticles.transform.localScale = new Vector3(20, 20, 20);
            newParticles.Play();
            _bhapticManager.RequestStartEvent(BhapticsEvent.GETDAMAGE);
        }
    }
}
