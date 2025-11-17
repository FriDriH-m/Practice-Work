using System.Collections;
using UnityEngine;
using Utils;

public class BotGunsControl : MonoBehaviour
{
    private int _reloadTime = 10;
    private Coroutine _reloadCoroutine;
    private GunsShootingSystem _gunsShootingSystem;
    void Start()
    {
        _gunsShootingSystem = GetComponent<GunsShootingSystem>();
        _gunsShootingSystem.Init(GetComponentInParent<AirplanePhysics>());
    }
    public void ReloadGuns()
    {
        _gunsShootingSystem.Reload();
    }
    public void Shoot()
    {
        if (_reloadCoroutine != null) return;
        if (_gunsShootingSystem.Ammo.Count == 0)
        {
            _reloadCoroutine = StartCoroutine(Reloading());
        }
        _gunsShootingSystem.Shoot();
        
    }
    public int GetBulletSpeed()
    {
        return _gunsShootingSystem.BulletSpeed;
    }
    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(_reloadTime);
        ReloadGuns();
        _reloadCoroutine = null;
    }
}
