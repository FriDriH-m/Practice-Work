using UnityEngine;
using Utils;

public class BotGunsControl : MonoBehaviour
{
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
        _gunsShootingSystem.Shoot();
    }
}
