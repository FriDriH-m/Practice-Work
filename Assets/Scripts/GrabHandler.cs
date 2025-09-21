using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public interface IHandRotator 
{
    public void SetHand(Transform hand);
}

public class GrabHandler<T> : MonoBehaviour where T : MonoBehaviour, IHandRotator
{
    private void Awake()
    {
        XRSimpleInteractable grabInteractable = GetComponent<XRSimpleInteractable>();
        grabInteractable.selectEntered.AddListener(Selected);
        grabInteractable.selectExited.AddListener(Unselected);
    }
    private void Selected(SelectEnterEventArgs args)
    {
        if (transform.TryGetComponent<T>(out T component))
        {
            component.SetHand(args.interactorObject.transform);
        }
    }

    private void Unselected(SelectExitEventArgs args)
    {
        if (transform.TryGetComponent<T>(out T component))
        {
            component.SetHand(null);
        }
    }
}
