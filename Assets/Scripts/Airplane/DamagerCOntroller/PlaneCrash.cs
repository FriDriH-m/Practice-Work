using Bhaptics.SDK2;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class PlaneCrash : MonoBehaviour
{
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private TextMeshProUGUI _crashText;
    [SerializeField] private TextMeshProUGUI _loseText;
    [SerializeField] private int _planesCount = 2;
    private float _textOpacity = 0;
    private Vector3 _spawnPosition;
    private Vector3 _spawnRotation;
    private BhapticManager _bhapticManager;

    private void Start()
    {
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation.eulerAngles;
        _bhapticManager = DIContainer.Instance.Get<BhapticManager>();
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
        if (_planesCount < 1)
        {
            StartCoroutine(GameOver());
            return;
        }
        StartCoroutine(StartRespawn());
    }
    private IEnumerator StartRespawn()
    {
        _bhapticManager.RequestStartEvent(BhapticsEvent.PLANECRASH);
        _crashText.gameObject.SetActive(true);
        _planesCount--;
        _crashText.text = "ÒÛ ÐÀÇÁÈËÑß \nÎñòàëîñü æèçíåé: " + _planesCount;
        _textOpacity = 0;
        DIContainer.Instance.Get<AirplanePhysics>("Player_Plane").SetThrust(0);
        DIContainer.Instance.Get<Transform>("Player_Thrust").localRotation = Quaternion.identity;
        while (_textOpacity <= 1)
        {
            _textOpacity += 0.2f * Time.deltaTime;
            _crashText.color = new Color(_textOpacity, _textOpacity, _textOpacity);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        transform.eulerAngles = _spawnRotation; 
        transform.position = _spawnPosition;
        _deathPanel.SetActive(false);
        
    }
    private IEnumerator GameOver()
    {
        _bhapticManager.RequestStartEvent(BhapticsEvent.PLANECRASH);
        _loseText.gameObject.SetActive(true);
        _textOpacity = 0;
        DIContainer.Instance.Get<AirplanePhysics>("Player_Plane").SetThrust(0);
        DIContainer.Instance.Get<Transform>("Player_Thrust").localRotation = Quaternion.identity;
        while (_textOpacity <= 1)
        {
            _textOpacity += 0.2f * Time.deltaTime;
            _crashText.color = new Color(_textOpacity, _textOpacity, _textOpacity);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadSceneAsync("SampleScene");
    }
}
