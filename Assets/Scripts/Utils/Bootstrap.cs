using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private void Awake()
    {
        var xrInput = new XRInput();
        xrInput.Enable();

        DIContainer.Instance.Register<XRInput>(xrInput, isSingleton: true);
    }
}
