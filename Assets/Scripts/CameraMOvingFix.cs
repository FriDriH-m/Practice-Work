using UnityEngine;

public class FixHmdLocalXZ : MonoBehaviour
{
    [Tooltip("Трансформ HMD (камера). Если пусто, попытается взять Camera.main.")]
    public Transform hmdTransform;

    [Tooltip("Разрешать ли вертикальное смещение (Y). Если false — и Y будет фиксирован).")]
    public bool allowVertical = true;

    private Vector3 initialLocalPos;

    void Start()
    {
        if (hmdTransform == null && Camera.main != null)
            hmdTransform = Camera.main.transform;

        if (hmdTransform == null)
        {
            Debug.LogError("[FixHmdLocalXZ] HMD not assigned and no Camera.main found.");
            enabled = false;
            return;
        }

        initialLocalPos = hmdTransform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 lp = hmdTransform.localPosition;
        lp.x = initialLocalPos.x;
        lp.z = initialLocalPos.z;
        if (!allowVertical) lp.y = initialLocalPos.y;
        hmdTransform.localPosition = lp;
    }
}
