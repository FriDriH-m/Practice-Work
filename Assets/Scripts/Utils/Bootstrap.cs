using Bhaptics.SDK2;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class Bootstrap : MonoBehaviour
{
    private BhapticManager _bhapticManager;
    private void Awake()
    {
        var xrInput = new XRInput();
        xrInput.Enable();
        _bhapticManager = new BhapticManager();

        DIContainer.Instance.Register<BhapticManager>(_bhapticManager, isSingleton: true);
        DIContainer.Instance.Register<MatchManager>(new MatchManager(), isSingleton: true);

        DIContainer.Instance.Register<XRInput>(xrInput, isSingleton: true);
    }
    private void Start()
    {        
        _bhapticManager.Initialize();
    }
    private void Update()
    {        
        
    }
}
