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
    public int _damage { get; private set; }

    private void Awake()
    {
        _isDamaged = false;
        _damage = 50;
    }
    public void MakeWasDamaged() 
    {
        _isDamaged = true;
    }
}
