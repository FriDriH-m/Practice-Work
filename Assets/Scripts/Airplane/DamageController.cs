using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private PartOfPlane _part;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Debug.Log("”рон по " + _part);
        }
    }
}
public enum PartOfPlane 
{ 
    Engine, Fuselage, Wings
}
