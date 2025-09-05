using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class YokeGrabHandler : MonoBehaviour
{
    public ConfigurableJoint yokeJoint; // —сылка на Configurable Joint на Yoke
    private JointDrive originalXDrive;
    private JointDrive originalYZDrive;

    void Start()
    {
        var grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        originalXDrive = yokeJoint.angularXDrive;
        originalYZDrive = yokeJoint.angularYZDrive;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        GameObject interactor = args.interactorObject.transform.gameObject;
        interactor.GetComponent<BoxCollider>().enabled = false;
        JointDrive freeDrive = new JointDrive { positionSpring = 0, positionDamper = 0, maximumForce = 0 };
        yokeJoint.angularXDrive = freeDrive;
        yokeJoint.angularYZDrive = freeDrive;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        GameObject interactor = args.interactorObject.transform.gameObject;
        interactor.GetComponent<BoxCollider>().enabled = true;
        yokeJoint.angularXDrive = originalXDrive;
        yokeJoint.angularYZDrive = originalYZDrive;
        yokeJoint.targetRotation = Quaternion.identity; // —брос в 0
    }
}
