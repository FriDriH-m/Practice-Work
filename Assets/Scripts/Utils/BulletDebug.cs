using UnityEngine;

public class BulletDebug : MonoBehaviour
{
    private Vector3 _lastPos;
    private float _lifeTime;

    private void OnEnable()
    {
        _lastPos = transform.position;
        _lifeTime = 0f;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 currentPos = transform.position;

        float worldSpeed = (currentPos - _lastPos).magnitude / dt; // м/с
        _lifeTime += dt;

        if (_lifeTime < 0.5f) // чтобы не спамить слишком долго
        {
            Debug.Log($"[BulletDebug] worldSpeed = {worldSpeed:F1} m/s");
        }

        _lastPos = currentPos;
    }
}
