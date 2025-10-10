using UnityEngine;

public class AimTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Hit the target!");
        }
    }
}
