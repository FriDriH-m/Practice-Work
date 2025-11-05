using Interfaces;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [Header("Reference Value")]
    [SerializeField] private MonoBehaviour _providerBehaviour;    
    [SerializeField] private int _minValue;
    [SerializeField] private int _maxValue;
    [SerializeField] private int _maxDegrees;
    private IIndicatorProvider _valueProvider;

    private float _startZ;
    private void Awake()
    {
        _startZ = transform.localEulerAngles.z;
        if (_providerBehaviour is IIndicatorProvider casted)
        {
            _valueProvider = casted;
        }
        else if (_providerBehaviour != null)
        {
            Debug.LogWarning($"{name}: assigned provider does not implement IIndicatorProvider.");
        }
    }

    private void UpdateRotation()
    {
        float targetRotation = _maxDegrees * Mathf.InverseLerp(_minValue, _maxValue, _valueProvider.GetValue());

        transform.localEulerAngles = new Vector3(0, 0, _startZ - targetRotation);
    }

    private void FixedUpdate()
    {
        UpdateRotation();
    }
}
