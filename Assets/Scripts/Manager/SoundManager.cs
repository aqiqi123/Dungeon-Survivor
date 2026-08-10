using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayerStats.Instance.OnGoldChanged += PlayerStats_OnGoldChanged;
        LevelManager.Instance.OnLevelUp += LevelManager_OnLevelUp;
    }

    private void LevelManager_OnLevelUp(int obj)
    {
        PlaySound(audioClipRefsSO.Levelup);
    }

    private void PlayerStats_OnGoldChanged()
    {
        PlaySound(audioClipRefsSO.Coin);
    }

    private void PlaySound(AudioClip audioClip, float volume = 0.8f)
    {
        audioSource.PlayOneShot(audioClip, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, float volume = 0.8f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], volume);
    }
}
