using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonReaction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _audioSource.clip = _audioClip[0];
        _audioSource.Play();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.clip = _audioClip[1];
        _audioSource.Play();
    }
}
