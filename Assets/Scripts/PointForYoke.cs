using UnityEngine;

public class PointForYoke : MonoBehaviour
{
    [SerializeField] private Transform hand;
    void Update()
    {
        transform.position = hand.position;
    }
}
