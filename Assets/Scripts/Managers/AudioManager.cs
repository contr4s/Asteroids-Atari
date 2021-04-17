using System;
using UnityEngine;

[Serializable]
public class AudioManager
{
    [SerializeField] private float _volume;
    [SerializeField] private AudioClip _shot;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private AudioSource _audioSource;

    public void PlayShootSound()
    {
        _audioSource.PlayOneShot(_shot, _volume);
    }

    public void PlayExplosionSound()
    {
        _audioSource.PlayOneShot(_explosion, _volume);
    }
}
