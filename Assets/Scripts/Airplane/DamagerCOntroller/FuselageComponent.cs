using UnityEngine;
using Interfaces;

public class FuselageComponent : MonoBehaviour, IDamagable
{
    [SerializeField] private int _health;
    [SerializeField] private ParticleSystem _brokeParticles;
    [SerializeField] private ParticleSystem _hitParticle;
    private AirplanePhysics _airplanePhysics;

    private void Awake()
    {
        _airplanePhysics = GetComponentInParent<AirplanePhysics>();
        if (_airplanePhysics == null)
        {
            Debug.LogWarning("DamageController: did not find AirplanePhysics");
        }
    }
    public void TakeDamage(int damageCount)
    {
        if (_health - damageCount > 0)
        {
            _health -= damageCount;
        }
        else
        {
            _airplanePhysics.BrokeEngine();
            var newParticles = Instantiate(_brokeParticles, transform.position, transform.rotation, transform);
            newParticles.transform.localScale = new Vector3(20, 20, 20);
            newParticles.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet>(out var bullet) && !bullet._isDamaged)
        {
            Debug.Log("Попал");
            TakeDamage(bullet._damage);
            var newParticles = Instantiate(_hitParticle, transform.position, transform.rotation, transform);
            newParticles.Play();
        }
    }
}
