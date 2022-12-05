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
        _audioSource.PlayOneShot(_clips[index]);

        index = (index + 1) % _clips.Count;
    }
}
