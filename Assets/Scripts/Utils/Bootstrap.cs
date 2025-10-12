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
        //var asset = xrInput.asset;
        //foreach (var map in asset.actionMaps)
        //{
        //    foreach (var action in map.actions)
        //    {
        //        var a = action; // локальная копия для замыкания
        //        a.started += ctx => Debug.Log($"[Action START] {map.name}/{a.name} = {ctx.ReadValueAsObject()}");
        //        a.performed += ctx => Debug.Log($"[Action PERF ] {map.name}/{a.name} = {ctx.ReadValueAsObject()}");
        //        a.canceled += ctx => Debug.Log($"[Action END  ] {map.name}/{a.name} = {ctx.ReadValueAsObject()}");
        //    }
        //}

        DIContainer.Instance.Register<XRInput>(xrInput, isSingleton: true);
    }
    private void Start()
    {        
        _bhapticManager.Initialize();
    }
    private void Update()
    {        
        //DIContainer.Instance.Get<BhapticManager>().Debugging();
        //DIContainer.Instance.Debugging();
    }
}
