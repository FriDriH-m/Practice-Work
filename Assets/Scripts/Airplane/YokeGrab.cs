using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class YokeGrabHandler : MonoBehaviour
{
    private void Awake()
    {
        XRSimpleInteractable grabInteractable = GetComponent<XRSimpleInteractable>();
        grabInteractable.selectEntered.AddListener(Selected);
        grabInteractable.selectExited.AddListener(Unselected);
    }
    private void Selected(SelectEnterEventArgs args)
    {
        if (transform.TryGetComponent<YokeLookAtHand>(out YokeLookAtHand component))
        {
            component.SetHand(args.interactorObject.transform);
        }
    }

    private void Unselected(SelectExitEventArgs args)
    {
        if (transform.TryGetComponent<YokeLookAtHand>(out YokeLookAtHand component))
        {
            component.SetHand(null);
        }
    }
}
