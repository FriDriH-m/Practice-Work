using Interfaces;
using UnityEngine;
using Utils;

public class PartHealthProvider : MonoBehaviour, IIndicatorProvider
{
    [SerializeField] private PartComponent _partComponent;
    public float GetValue()
    {
        return _partComponent.Health;
    }
}
