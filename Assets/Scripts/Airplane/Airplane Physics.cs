using Bhaptics.SDK2;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using Utils;

public class AirplanePhysics : MonoBehaviour
{
    [SerializeField] private float _pitchDivider;
    [SerializeField] private float _sterringDivider;
    [SerializeField] private Transform _yoke;
    [SerializeField] private float _maxThrust = 100f;
    [SerializeField, Range(0, 1)] private float _thrust;
    [SerializeField] private float _liftPower;
    [SerializeField] private float _rudderPower;
    [SerializeField] private AnimationCurve _liftCoefficient;
    [SerializeField] private AnimationCurve _rubberCoefficient;
    [SerializeField] private float _inducedDrag;
    [SerializeField] private float _yawPower = 1f;
    [SerializeField] private AnimationCurve _yawLimiter;
    [SerializeField] private AnimationCurve _sterringPower;

    [Header("Drag coefficients")]
    [SerializeField] private AnimationCurve _dragForward;
    [SerializeField] private AnimationCurve _dragBackward;
    [SerializeField] private AnimationCurve _dragTop;
    [SerializeField] private AnimationCurve _dragBottom;
    [SerializeField] private AnimationCurve _dragLeft;
    [SerializeField] private AnimationCurve _dragRight;

    private Rigidbody _rigidbody;
    private XRInput _xrInput;
    private BhapticManager _bhapticManager;

    private bool _isEngineOn;
    private float _yawInput;
    private float _angleOfAttack;
    private float _angleOfAttackYaw;
    private Vector3 _localVelocity;
    private Vector3 _localAngularVelocity;
    private Vector3 _localGForce;
    private Vector3 _lastVelocity;
    private Vector3 _yawVelocity;
    private Vector3 _sterringInput;

    public Vector3 LocalVelocity => _localVelocity;
    public Vector3 LocalAngularVelocity => _localAngularVelocity;
    public Vector3 LocalGForce => _localGForce;
    public float Thrust => _thrust;

    //------For Gizmos visualization------
    private Vector3 _lastLift;
    private Vector3 _lastInducedDrag;
    private Vector3 _lastDrag;
    private Vector3 _yawForce;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
    }
    private void Start()
    {
        _xrInput = DIContainer.Instance.Get<XRInput>();
        _bhapticManager = DIContainer.Instance.Get<BhapticManager>();

        _xrInput.XRIHead.YawInput.performed += ctx => _yawInput = ctx.ReadValue<float>();
        _xrInput.XRIHead.YawInput.canceled += ctx => _yawInput = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 worldForward = transform.TransformDirection(Vector3.forward) * 15;
        Gizmos.DrawLine(transform.position, transform.position + worldForward);

        Gizmos.color = Color.green;
        Vector3 worldLocalVelocity = transform.TransformDirection(_localVelocity);
        Gizmos.DrawLine(transform.position, transform.position + worldLocalVelocity);

        Gizmos.color = Color.red;
        Vector3 worldLocalAngularVelocity = transform.TransformDirection(_localAngularVelocity);
        Gizmos.DrawLine(transform.position, transform.position + worldLocalAngularVelocity);
        //if (Application.isPlaying)
        //{
        //    // Визуализация подъемной силы (зеленый)
        //    Gizmos.color = Color.green;
        //    Vector3 worldLift = transform.TransformDirection(_lastLift) * 0.01f;
        //    Gizmos.DrawLine(transform.position, transform.position + worldLift);
        //    Gizmos.DrawSphere(transform.position + worldLift, 0.1f);

        //    // Визуализация индуктивного сопротивления (желтый)
        //    Gizmos.color = Color.yellow;
        //    Vector3 worldInducedDrag = transform.TransformDirection(_lastInducedDrag) * 0.01f;
        //    Gizmos.DrawLine(transform.position, transform.position + worldInducedDrag);
        //    Gizmos.DrawSphere(transform.position + worldInducedDrag, 0.1f);

        //    // Визуализация сопротивления (красный)
        //    Gizmos.color = Color.red;
        //    Vector3 worldDrag = transform.TransformDirection(_lastDrag) * 0.01f;
        //    Gizmos.DrawLine(transform.position, transform.position + worldDrag);
        //    Gizmos.DrawSphere(transform.position + worldDrag, 0.1f);

        //    Gizmos.color = Color.blue;
        //    Vector3 worldRudder = transform.TransformDirection(_yawForce) * 0.01f;
        //    Gizmos.DrawLine(transform.position, transform.position + worldRudder);
        //    Gizmos.DrawSphere(transform.position + worldRudder, 0.1f);

        //    // Подписи для сил
        //    UnityEditor.Handles.Label(transform.position + worldLift, "Lift");
        //    UnityEditor.Handles.Label(transform.position + worldInducedDrag, "Induced Drag");
        //    UnityEditor.Handles.Label(transform.position + worldDrag, "Drag");
        //    UnityEditor.Handles.Label(transform.position + worldRudder, "Rudder Force");
        //}
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        UpdateCurrentState();

        CalculateDrag();
        CalculateAngleOfAttack();
        CalculateGForce(deltaTime);

        UpdateLift();
        UpdateSteering(deltaTime, _sterringInput);

        _yawVelocity = new Vector3(0, _yawInput * (_yawPower * _yawLimiter.Evaluate(_localVelocity.magnitude * 3.6f)), 0);

        _rigidbody.AddRelativeForce(Vector3.forward * _thrust * _maxThrust); // Thrust (тяга)
        _rigidbody.AddRelativeTorque(_yawVelocity, ForceMode.Force); // Yaw (рысканье)
        //Debug.Log(_angleOfAttackYaw + " | " + _localGForce.magnitude);
    }

    public void SetSteeringInput(Vector3 newInpit)
    {
        _sterringInput = newInpit;
    }

    public void SetThrust(float angle)
    {        
        if (angle <= 0.1f)
        {
            if (_isEngineOn) _bhapticManager.RequestStartEvent(BhapticsEvent.ENGINESTOP);
                
            _isEngineOn = false;
            _thrust = 0;
            return;
        }
        if (angle >= 60f)
        {
            _thrust = 1f;
            _isEngineOn = true;
            return;
        }
        _isEngineOn = true;
        _bhapticManager.RequestStartEvent(BhapticsEvent.ENGINESTART);
        _thrust = angle / 60;
    }

    private void UpdateCurrentState()
    {
        Quaternion inverseRotation = Quaternion.Inverse(_rigidbody.rotation);

        _localVelocity = inverseRotation * _rigidbody.linearVelocity;
        _localAngularVelocity = inverseRotation * _rigidbody.angularVelocity;
    }

    private void UpdateLift()
    {
        if (_localVelocity.sqrMagnitude < 1f) return;

        Vector3 lift = CalculateLift(_angleOfAttack, Vector3.right, _liftCoefficient, _liftPower);
        Vector3 yawForce = CalculateLift(_angleOfAttackYaw, Vector3.up, _rubberCoefficient, _rudderPower);

        _lastLift = lift;
        _yawForce = yawForce;

        _rigidbody.AddRelativeForce(lift);
        _rigidbody.AddRelativeForce(yawForce);
    }

    private void UpdateSteering(float deltaTime, Vector3 input)
    {
        Vector3 euler = input;

        float signedX = (euler.x > 180f) ? euler.x - 360f : euler.x;  
        float signedZ = (euler.z > 180f) ? euler.z - 360f : euler.z; 
        
        if (signedX <= 1f && signedX >= -1f) signedX = 0f;
        if (signedZ <= 1f && signedZ >= -1f) signedZ = 0f;

        Vector3 targetTorque = new Vector3(signedZ/ _pitchDivider, 0, -signedX/ _sterringDivider);

        _rigidbody.AddRelativeTorque(targetTorque * _sterringPower.Evaluate(_localVelocity.magnitude * 3.6f), ForceMode.VelocityChange);
    }

    private float CalculateSteering(float deltaTime, float angularVelocity, float targetVelocity, float acceleration)
    {
        float steering = angularVelocity - targetVelocity;
        float acltion = acceleration * deltaTime;
        return Mathf.Clamp(steering, -acltion, acltion);
    }

    private void CalculateAngleOfAttack()
    {
        if (_localVelocity.sqrMagnitude < 0.1f)
        {
            _angleOfAttack = 0;
            _angleOfAttackYaw = 0;
            return;
        }
        // находим угол атаки. он находится так: берем ось (y), которая смотрит вперед, берем ось (z), которая перпендикюлярна крыльям
        _angleOfAttack = Mathf.Atan2(-_localVelocity.y, _localVelocity.z) * Mathf.Rad2Deg;
        // для Yaw (рысканья) нужно взять ось вперед (z) и ось вдоль крыльев (x)
        _angleOfAttackYaw = Mathf.Atan2(_localVelocity.x, _localVelocity.z) * Mathf.Rad2Deg; 
    }
    private void CalculateGForce(float deltaTime)
    {
        var inverseRotation = Quaternion.Inverse(_rigidbody.rotation);
        var acceleration = (_localVelocity - _lastVelocity) / deltaTime;
        _localGForce = inverseRotation * acceleration;
        //Debug.Log(_localGForce.magnitude);
        _lastVelocity = _localVelocity;
    }
    private void CalculateDrag()
    {
        Vector3 lclVelocity = _localVelocity;
        float lclVelocity2 = lclVelocity.sqrMagnitude;

        Vector3 dragCoefficient = Scale6(
            lclVelocity.normalized,
            _dragRight.Evaluate(Mathf.Abs(lclVelocity.x)), _dragLeft.Evaluate(Mathf.Abs(lclVelocity.x)),
            _dragTop.Evaluate(Mathf.Abs(lclVelocity.y)), _dragBottom.Evaluate(Mathf.Abs(lclVelocity.y)),
            _dragForward.Evaluate(Mathf.Abs(lclVelocity.z)), _dragBackward.Evaluate(Mathf.Abs(lclVelocity.z))
         );

        var drag = 0.5f * dragCoefficient.magnitude * lclVelocity2 * -lclVelocity; // формула D = 1/2 * V^2 * Cd

        _lastDrag = drag;

        _rigidbody.AddRelativeForce(drag);
    }

    private Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis, AnimationCurve ribberAOACurve, float liftPower)
    {
        var liftVelocity = Vector3.ProjectOnPlane(_localVelocity, rightAxis);
        Vector3 lclVelocity = _localVelocity;
        float lclVelocity2 = lclVelocity.sqrMagnitude;

        float liftCoeficient = ribberAOACurve.Evaluate(angleOfAttack);
        float liftForce = 0.5f * lclVelocity2 * liftCoeficient * liftPower;

        Vector3 liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        Vector3 lift = liftDirection * liftForce;

        
        float inducedDragForce = liftCoeficient * liftCoeficient * _inducedDrag;
        //Debug.Log("Induced Drag of lift:" + inducedDragForce);
        Vector3 inducedDragDirection = -liftVelocity.normalized;
        Vector3 inducedDrag = inducedDragDirection * inducedDragForce;

        _lastInducedDrag = inducedDrag;

        return lift + inducedDrag;
    }

    public static Vector3 Scale6(Vector3 value, 
        float posX, float negX,
        float posY, float negY,
        float posZ, float negZ)
    {
        Vector3 result = value;
        if (result.x > 0)
        {
            result.x *= posX;
        }
        else if (result.x < 0)
        {
            result.x *= negX;
        }

        if (result.y > 0)
        {
            result.y *= posY;
        }
        else if (result.y < 0)
        {
            result.y *= negY;
        }

        if (result.z > 0)
        {
            result.z *= posZ;
        }
        else if (result.z < 0)
        {
            result.z *= negZ;
        }
        return result;
    }
}