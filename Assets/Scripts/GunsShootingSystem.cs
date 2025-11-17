using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class GunsShootingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _objectPool;
    [SerializeField] private Transform[] _gunsPositions;
    [SerializeField] private float _shootingSpeed = 1f;
    [SerializeField] private float _reloadSpeed = 15f;
    [SerializeField] private int _bulletSpeed = 300;
    [SerializeField] private bool isPlayer = true;
    private float _reloadTimer;
    private bool _isReloading = false;
    private Queue<GameObject> _ammo = new Queue<GameObject>();
    private Queue<GameObject> _shootedAmmo = new Queue<GameObject>();
    private Coroutine _shootCoroutine;
    private AirplanePhysics _airplanePhysics;
    public int BulletSpeed => _bulletSpeed;
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
    }
    public void Init(AirplanePhysics airplanePhysics)
    {
        _airplanePhysics = airplanePhysics;
        if (_airplanePhysics == null)
        {
            Debug.LogError("[GunsShootingSystem] AirplanePhysics не найден на объекте.");
        }
    }
    public void Reload()
    {
        _isReloading = true;
        _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0)
        {
            foreach (var ammo in _shootedAmmo)
            {
                var rb = ammo.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                ammo.SetActive(false);
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
            Transform airplane = transform.parent;
            for (int i = 0; i < _gunsPositions.Length; i++)
            {
                newBullet = _ammo.Dequeue();
                _shootedAmmo.Enqueue(newBullet);

                newBullet.transform.position = _gunsPositions[i].position;
                newBullet.SetActive(true);

                Rigidbody rigidbody = newBullet.GetComponent<Rigidbody>();

                rigidbody.linearVelocity = 
                    _airplanePhysics.GetComponent<Rigidbody>().GetPointVelocity(_gunsPositions[i].position) 
                    + (_gunsPositions[i].forward + airplane.up * 0.1f) 
                    * _bulletSpeed;
                rigidbody.angularVelocity = Vector3.zero;   
            }
        }
        yield return new WaitForSeconds(_shootingSpeed);
        _shootCoroutine = null;
    }
    //private void OnDrawGizmos()
    //{
    //    foreach (var item in _gunsPositions)
    //    {
    //        Debug.Log(item.ToString());
    //        Gizmos.color = Color.red;
    //        Vector3 worldForward = transform.TransformDirection(Vector3.forward) * 1000;
    //        Gizmos.DrawLine(item.position, item.position + worldForward);
    //    }        
    //}
    private void Update()
    {
        
    }
}
