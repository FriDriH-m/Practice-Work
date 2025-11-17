using UnityEngine;

public class GunsSpread : MonoBehaviour
{
    private Vector3 startRotation;
    private void Start()
    {
        startRotation = transform.localEulerAngles;
    }
    private void Update()
    {
        float spreadY = Random.Range(-0.7f, 0.7f);
        float spreadX = Random.Range(-0.7f, 0.7f);
        transform.localEulerAngles = startRotation + new Vector3(spreadX, spreadY, 0);
    }
}
