using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class YokeGrabHandler : MonoBehaviour
{
    void Start()
    {
        var grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        GameObject interactor = args.interactorObject.transform.gameObject;
        if (TryGetComponent<YokeLookAtHand>(out var yokeLookAtHand))
        {
            yokeLookAtHand.SetHandForYoke(interactor.transform);
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        GameObject interactor = args.interactorObject.transform.gameObject;
        if (TryGetComponent<YokeLookAtHand>(out var yokeLookAtHand))
        {
            yokeLookAtHand.SetHandForYoke(null);
        }
    }
}
