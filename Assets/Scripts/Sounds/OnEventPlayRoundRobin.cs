using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEventPlayRoundRobin : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _clips;
    private int index = 0;

    public void RoundRobinPlaySound()
    {
        if (_clips[index] == null)
            Debug.Log("This clip is null");
        _audioSource.PlayOneShot(_clips[index]);

        Debug.Log("I'm playing sound rn");

        index = (index + 1) % _clips.Count;
    }
}
