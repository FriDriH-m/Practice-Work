using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

public class RickRoll : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject rickRoll;
    [SerializeField] private AudioSource source;
    [SerializeField] private VideoPlayer videoPlayer;
    private Coroutine corr;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        source.enabled = false;
        player.parent = null;
        player.position = new Vector3(-19.98f, 84.22f, -9.59f);
        player.rotation = Quaternion.identity;
        videoPlayer.enabled = true;
    }
}
