using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneCrash : MonoBehaviour
{
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private TextMeshProUGUI _crashText;
    private float _textOpacity = 0;
    private Vector3 _spawnPosition;
    private Vector3 _spawnRotation;

    private void Start()
    {
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation.eulerAngles;
        //Crash();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var n = collision.GetContact(0).normal; 
        float vN = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, n)); 

        if (vN > 12f) Crash();
    }
    private void Crash()
    {
        _deathPanel.SetActive(true);
        StartCoroutine(StartRespawn());
    }
    private IEnumerator StartRespawn()
    {
        _textOpacity = 0;
        while (_textOpacity <= 1)
        {
            _textOpacity += 0.3f * Time.deltaTime;
            _crashText.color = new Color(_textOpacity, _textOpacity, _textOpacity);
            yield return null;
        } 
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        transform.eulerAngles = _spawnRotation; 
        transform.position = _spawnPosition;
        _deathPanel.SetActive(false);
    }
}
