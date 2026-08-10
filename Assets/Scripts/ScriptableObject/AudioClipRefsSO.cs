using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRefsSO",menuName ="SOs/AudioClipRefs")]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] Coin;

    public AudioClip[] Levelup;
}
