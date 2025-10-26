using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private PartOfPlane _part;
    private AirplanePhysics _airplanePhysics;

    private void Awake()
    {
        
        _airplanePhysics = GetComponentInParent<AirplanePhysics>();
        if (_airplanePhysics == null)
        {
            Debug.LogWarning("DamageController: did not find AirplanePhysics");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Контакт");
        if (other.TryGetComponent<IBullet>(out IBullet bullet) && !bullet._isDamaged)
        {
            Debug.Log("Урон по" + bullet._damage + ": " + _part);
        }
    }
}
public enum PartOfPlane 
{ 
    Engine, Fuselage, Wings
}
