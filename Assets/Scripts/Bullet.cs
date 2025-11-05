using Interfaces;
using UnityEngine;

public interface IBullet 
{
    public bool _isDamaged { get; }
    public int _damage { get; }
    public void MakeWasDamaged();
}

public class Bullet : MonoBehaviour, IBullet
{
    public bool _isDamaged { get; private set; }
    [field: SerializeField] public int _damage { get; private set; }

    private void Awake()
    {
        _isDamaged = false;
    }
    public void MakeWasDamaged() 
    {
        _isDamaged = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.TakeDamage(_damage);
        }
        gameObject.SetActive(false);
    }
}
