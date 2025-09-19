using UnityEngine;


public class YokeLookAtHand : MonoBehaviour
{
    [SerializeField] Transform yoke;
    [SerializeField] Vector3 forwardOffsetEuler = Vector3.zero;
    Transform hand;
    public void SetHandForYoke(Transform hand)
    {
        this.hand = hand;
    }
    private void YokeRotation()
    {
        if (hand == null) return;

        transform.LookAt(hand, yoke.right);
        transform.rotation *= Quaternion.Euler(forwardOffsetEuler);

        Vector3 local = transform.localEulerAngles;
        float x = Mathf.DeltaAngle(0f, local.x);
        float y = Mathf.DeltaAngle(0f, local.y);
        float z = Mathf.DeltaAngle(0f, local.z);

        // Клэмпим X и Z в [-10, 10]
        float targetX = Mathf.Clamp(x, -10f, 10f);
        float targetZ = Mathf.Clamp(z, -10f, 10f);

        transform.localEulerAngles = new Vector3(targetX, y, targetZ);
    }
    void LateUpdate()
    {
        YokeRotation();
    }
}

