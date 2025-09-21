using UnityEngine;

public class LeverRotator : MonoBehaviour, IHandRotator
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Vector3 forwardOffsetEuler = Vector3.zero;
    private Transform _hand = null;
    public void SetHand(Transform hand)
    {
        _hand = hand;
    }
    private void Update()
    {
        if (_hand == null) return;

        transform.LookAt(_hand, transform.right);
        transform.rotation *= Quaternion.Euler(forwardOffsetEuler);

        Vector3 local = transform.localEulerAngles;
        float x = Mathf.DeltaAngle(0f, local.x);
        
        x = Mathf.Clamp(x, 0, 60);

        transform.localEulerAngles = new Vector3(x, 0, 0);
    }
}
