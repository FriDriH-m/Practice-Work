using UnityEngine;

public class AirplaneControl : MonoBehaviour
{
    /*
     * transform.right - вперед
     * transform.forward - вверх
     */

    private Vector3 _currentForce;
    private Rigidbody _rigidBody;
    [SerializeField] private float _power;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _linearDamping;
    [SerializeField] Transform _yoke;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void SetForce()
    {
        bool isPressedW = Input.GetKey(KeyCode.W);
        if (isPressedW)
        {
            _currentForce.z += _power * Time.deltaTime;
        }
        else if (_currentForce.z > 0)
        {
            _currentForce.z -= _linearDamping * Time.deltaTime;
        }
        else
        {
            _currentForce = Vector3.zero;
        }
    }

    public void Rotate()
    {
        transform.Rotate(_yoke.rotation.eulerAngles);
    }

    private void Update()
    {
        SetForce();
        _rigidBody.AddForce(transform.right * Mathf.Min(_currentForce.z, _maxForce), ForceMode.Force);
    }
}
