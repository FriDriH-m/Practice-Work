using UnityEngine;

public class AirplanePhysics : MonoBehaviour
{
    [SerializeField]
    private bool _dragOn;
    [SerializeField] 
    private float _maxThrust = 100f;
    [SerializeField, Range(0, 1)] 
    private float _thrust;
    [SerializeField] 
    private float _angularVelocityMultiply;
    [SerializeField]
    private float _liftPower;
    [SerializeField]
    private AnimationCurve _coefficientOfLift;
    [SerializeField]
    private float _inducedDrag;

    [Header("Drag coefficient")]
    //[SerializeField]
    private AnimationCurve _dragForward = AnimationCurve.Linear(0, 0, 700, 0.5f);
    //[SerializeField]
    private AnimationCurve _dragBackward = AnimationCurve.Linear(0, 0, 700, 1f);
    //[SerializeField]
    private AnimationCurve _dragTop = AnimationCurve.Linear(0, 0, 700, 3f);
    //[SerializeField]
    private AnimationCurve _dragBottom = AnimationCurve.Linear(0, 0, 700, 3f);
    //[SerializeField]
    private AnimationCurve _dragLeft = AnimationCurve.Linear(0, 0, 700, 2f);
    //[SerializeField] 
    private AnimationCurve _dragRight = AnimationCurve.Linear(0, 0, 700, 2f);

    private Rigidbody _rigidbody;

    private float velocity;

    private Vector3 _localVelocity;
    private Vector3 _localAngularVelocity;
    private float _angleOfAttack;
    private float _angleOfAttackYaw;
    private Vector3 _localGForce;

    private Vector3 _lastVelocity;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        UpdateCurrentState();
        if (_dragOn)
        {
            CalculateDrag();
        }        
        CalculateAngleOfAttack();
        CalculateGForce(deltaTime);

        UpdateLift();


        _rigidbody.AddRelativeForce(Vector3.forward * _thrust * _maxThrust); // Thrust (тяга)
        
        _rigidbody.AddRelativeTorque(Vector3.forward * _angularVelocityMultiply);    
        
    }

    private void UpdateCurrentState()
    {
        Quaternion inverseRotation = Quaternion.Inverse(_rigidbody.rotation);

        _localVelocity = inverseRotation * _rigidbody.linearVelocity;
        _localAngularVelocity = inverseRotation * _rigidbody.angularVelocity;
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

        //Debug.Log(_angleOfAttack);
        //Debug.Log(_angleOfAttackYaw);
        //Debug.Log(_localGForce.magnitude);
    }
    private void CalculateGForce(float deltaTime)
    {
        var inverseRotation = Quaternion.Inverse(_rigidbody.rotation);
        var acceleration = (_localVelocity - _lastVelocity) / deltaTime;
        _localGForce = inverseRotation * acceleration;
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
            _dragForward.Evaluate(Mathf.Abs(lclVelocity.z)), _dragBackward.Evaluate(Mathf.Abs(lclVelocity.z)));

        var drag = 0.5f * dragCoefficient.magnitude * lclVelocity2 * -lclVelocity; // формула D = 1/2 * V^2 * Cd
        
        _rigidbody.AddRelativeForce(drag);
    }

    private void UpdateLift()
    {
        if (_localVelocity.sqrMagnitude < 1f) return;

        Vector3 lift = CalculateLift(_angleOfAttack, Vector3.right);
        Vector3 yawForce = CalculateLift(_angleOfAttackYaw, Vector3.up);

        Debug.Log("" + _localVelocity.magnitude * 3.6f + " | AoA = " + _angleOfAttack + " | Lift = " + lift.magnitude);

        _rigidbody.AddRelativeForce(lift);
    }

    private Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis)
    {
        var liftVelocity = Vector3.ProjectOnPlane(_localVelocity, rightAxis);
        Vector3 lclVelocity = _localVelocity;
        float lclVelocity2 = lclVelocity.sqrMagnitude;

        float liftCoeficient = _coefficientOfLift.Evaluate(_angleOfAttack);
        float liftForce = 0.5f * lclVelocity2 * liftCoeficient * _liftPower;

        Vector3 liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        Vector3 lift = liftDirection * liftForce;

        float inducedDragForce = liftCoeficient * liftCoeficient * _inducedDrag;
        Vector3 inducedDragDirection = -liftVelocity.normalized;
        Vector3 inducedDrag = inducedDragDirection * inducedDragForce;

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