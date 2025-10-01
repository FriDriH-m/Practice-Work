using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.OpenXR.Input;

public class MenuInGame : MonoBehaviour
{
    [SerializeField] private Vector2 xRange = new Vector2(-40, 40);
    [SerializeField] private Vector2 yRange = new Vector2(-70, 70);
    [SerializeField] private Vector2 zRange = new Vector2(50, 150);
    [SerializeField] private GameObject _menu;
    [SerializeField] private Transform _hand;
    private XRInput _XRInput;
    private void Awake()
    {
        _XRInput = new XRInput();
        //_XRInput = ServiceLocator.Instance.Get<XRInput>();
        //if (_XRInput == null)
        //{
        //    Debug.LogError("MenuInGame: ServiceLocator does not contain a " + typeof(XRInput));
        //}
    }
    private void OnEnable()
    {
        _XRInput.Enable();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadSceneAsync("SampleScene");
    }
    private void Update()
    {
        Quaternion localRotation = _hand.localRotation;
        Vector3 euler = localRotation.eulerAngles;

        euler.z = (euler.z > 180) ? euler.z - 360 : euler.z;

        bool isInRange =
            euler.z >= zRange.x && euler.z <= zRange.y;

        if (isInRange)
        {
            _menu.SetActive(true);
        }
        else if (!isInRange)
        {
            _menu.SetActive(false);
        }
    }
}
