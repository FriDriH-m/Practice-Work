using Bots;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class BotCrashChecker : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    public event Action<string> OnCrash;
    private void OnCollisionEnter(Collision collision)
    {
        var n = collision.GetContact(0).normal;
        float vN = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, n));

        if (vN > 12f) Crash();
    }
    private void Crash()
    {
        var effect = Instantiate(_explosion, transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(7, 7, 7);

        Debug.Log("ֲûחנûג");
        OnCrash?.Invoke(gameObject.name);
        gameObject.SetActive(false);
    }
    private void Update()
    {
        

    }
}
