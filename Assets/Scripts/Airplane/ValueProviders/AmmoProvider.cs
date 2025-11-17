using UnityEngine;
using Interfaces;
using Unity.VisualScripting;
using Utils;

public class AmmoProvider : MonoBehaviour, IIndicatorProvider
{
    private GunsShootingSystem _playerBulletSystem;
    private void Start()
    {
        _playerBulletSystem = DIContainer.Instance.Get<GunsShootingSystem>("Player_Bullets");
    }
    public float GetValue()
    {
        return _playerBulletSystem.Ammo.Count;
    }
}
