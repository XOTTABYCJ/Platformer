using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour // Script for sounds
{
    public AudioSource audioSource;
    public AudioClip jumpSounds, coinsSound, winSound, loseSound, heartsound, keysound, hitsound, doorsound;

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSounds);
    }

    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinsSound);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void PlayHeartSound()
    {
        audioSource.PlayOneShot(heartsound);
    }

    public void PlayKeySound()
    {
        audioSource.PlayOneShot(keysound);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitsound);
    }

    public void PlayDoorSound()
    {
        audioSource.PlayOneShot(doorsound);
    }
}
