using UnityEngine;
using Utils;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private XRInput _xrInput;
    void Start()
    {
        _xrInput = DIContainer.Instance.Get<XRInput>();
    }

    
    void Update()
    {
        animator.Play("A", 0, _xrInput.XRILeftInteraction.SelectValue.ReadValue<float>());
        animator.Play("B", 1, _xrInput.XRIRightInteraction.SelectValue.ReadValue<float>());
    }
}
