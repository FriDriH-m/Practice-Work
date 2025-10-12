using UnityEngine;

public interface IDamager
{
    public void GetDamage(float damage);
}
public class AirplaneDamager : MonoBehaviour, IDamager
{
    private int _health = 100;
    public void GetDamage(float damage)
    {

    }
}
