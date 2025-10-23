using UnityEngine;
using Futurift;

[DisallowMultipleComponent]
public class FuturiftSimpleCompensator : MonoBehaviour
{
    [Header("References")]
    public FuturiftController futuriftController; 
    [SerializeField] private Transform cube;
    [SerializeField] private Transform rotatingObject;

    private float startX;
    private float startZ;

    private float xrX;
    private float xrZ;

    [SerializeField] private float smoothTime = 0.1f; 
    private float velX = 0f;
    private float velZ = 0f;

    void Start()
    {
        if (futuriftController == null)
        {
            Debug.LogError("[FuturiftSimpleCompensator] futuriftController не назначен.");
            enabled = false;
            return;
        }
        startX = rotatingObject.localEulerAngles.x;
        startZ = rotatingObject.localEulerAngles.z;

        xrX = transform.localPosition.x;
        xrZ = transform.localPosition.z;
    }

    void LateUpdate()
    {
        float pitch = -futuriftController.Pitch;
        float roll = futuriftController.Roll;

        float targetX = 0 - pitch * 1f;
        float targetZ = 0 + roll;

        float newX = Mathf.SmoothDampAngle(rotatingObject.localEulerAngles.x, targetX, ref velX, smoothTime);
        float newZ = Mathf.SmoothDampAngle(rotatingObject.localEulerAngles.z, targetZ , ref velZ, smoothTime) ;

        rotatingObject.localEulerAngles = new Vector3(newX, 0, newZ);

        Vector3 cubeLclPos = transform.parent.InverseTransformPoint(cube.position);
        transform.localPosition = new Vector3(cubeLclPos.x, transform.localPosition.y, cubeLclPos.z);
    }
}
