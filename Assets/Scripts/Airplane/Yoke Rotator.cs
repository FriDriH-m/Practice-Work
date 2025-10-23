using UnityEngine;
using Utils;
using Bhaptics.SDK2;


public class YokeRotator : MonoBehaviour, IHandRotator
{
    [SerializeField] private Transform yoke;
    [SerializeField] private Vector3 forwardOffsetEuler = Vector3.zero;
    private Transform hand;
    private BhapticManager _bhapticManager;
    public Transform ActiveHand => hand;
    private void Awake()
    {
        DIContainer.Instance.Register<YokeRotator>(this, "Plane_Yoke");
    }
    private void Start()
    {
        _bhapticManager = DIContainer.Instance.Get<BhapticManager>();
    }
    public void SetHand(Transform hand)
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

        var _motorsPowerX = (int)(100 * Mathf.InverseLerp(-10, 10, targetX));
        var _motorsPowerZ = (int)(100 * Mathf.InverseLerp(-10, 10, targetZ));
        var _finalPower = Mathf.Max(_motorsPowerX, _motorsPowerZ);

        switch (hand.tag) 
        {
            case ("LeftHand"):
                _bhapticManager.ActiveMotor(PositionType.GloveL, 0, new int[] { _finalPower, _finalPower, _finalPower, _finalPower, _finalPower, _finalPower });
                break;
            case ("RightHand"):
                _bhapticManager.ActiveMotor(PositionType.GloveR, 0, new int[] { _finalPower, _finalPower, _finalPower, _finalPower, _finalPower, _finalPower });
                break;
            default:
                break;
        }
    }
    void LateUpdate()
    {
        YokeRotation();
    }
}

