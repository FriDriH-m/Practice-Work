using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GunsShootingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _objectPool;
    [SerializeField] private Transform[] _gunsPositions;
    [SerializeField] private float _shootingSpeed = 1f;
    [SerializeField] private float _reloadSpeed = 15f;
    private float _reloadTimer;
    private bool _isReloading = false;
    private Queue<GameObject> _ammo = new Queue<GameObject>();
    private Queue<GameObject> _shootedAmmo = new Queue<GameObject>();
    private Coroutine _shootCoroutine;
    public Queue<GameObject> Ammo => _ammo;
    public float ReloadTimer => _reloadTimer;
    public bool IsReloading => _isReloading;

    private void Awake()
    {
        _reloadTimer = _reloadSpeed;
        for (int i = 0; i < 300; i++)
        {
            var obj = Instantiate(_objectPool);
            _ammo.Enqueue(obj);
            obj.SetActive(false);
        }
        DIContainer.Instance.Register<GunsShootingSystem>(this, "Player_Bullets");
    }
    public void Reload()
    {
        _isReloading = true;
        _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0)
        {
            foreach (var ammo in _shootedAmmo)
            {                
                _ammo.Enqueue(ammo);
            }
            _shootedAmmo.Clear();
            _reloadTimer = _reloadSpeed;
            _isReloading = false;
        }
    }
    public void Shoot()
    {
        if (_shootCoroutine == null)
        {
            _shootCoroutine = StartCoroutine(ShootCorroutine());
        }
    }
    private IEnumerator ShootCorroutine()
    {
        GameObject newBullet;
        if (_ammo.Count >= _gunsPositions.Length)
        {
            for (int i = 0; i < _gunsPositions.Length; i++)
            {
                newBullet = _ammo.Dequeue();
                _shootedAmmo.Enqueue(newBullet);
                newBullet.transform.position = _gunsPositions[i].position;
                newBullet.SetActive(true);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.parent.forward * 3f + new Vector3(0, 0.3f, 0), ForceMode.Impulse);
            }
        }
        yield return new WaitForSeconds(_shootingSpeed);
        _shootCoroutine = null;
    }
}
