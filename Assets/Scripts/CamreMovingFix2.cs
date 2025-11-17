using UnityEngine;
using Futurift;

[DisallowMultipleComponent]
public class FuturiftSimpleCompensator : MonoBehaviour
{
    [Header("References")]
    public FuturiftController futuriftController; 
    [SerializeField] private Transform cube;
    [SerializeField] private Transform rotatingObject;
    [SerializeField] private float smoothTime = 0.35f;
    [SerializeField] private CompensatorData data; //test
    
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
    }

    void LateUpdate()
    {
        float pitch = -futuriftController.Pitch;
        float roll = futuriftController.Roll;

        float targetX = 0 - pitch * (3 * data.PitchMultiplie);
        float targetZ = 0 + roll * (3 * data.RollMultiplie);

        float newX = Mathf.SmoothDampAngle(rotatingObject.localEulerAngles.x, targetX, ref velX, data.PitchSmoothTime);
        float newZ = Mathf.SmoothDampAngle(rotatingObject.localEulerAngles.z, targetZ , ref velZ, data.RollSmoothTime);

        rotatingObject.localEulerAngles = new Vector3(newX, 0, newZ);

        Vector3 cubeLclPos = transform.parent.InverseTransformPoint(cube.position);
        transform.localPosition = new Vector3(cubeLclPos.x, transform.localPosition.y, cubeLclPos.z);
    }
}
