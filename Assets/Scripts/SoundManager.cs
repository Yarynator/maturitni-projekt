using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public enum SoundType
    {
        Steps,
        Attack,
        Fireball,
        Cookie
    }


    public static SoundManager Instance { get; private set; }


    float volume;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("MusicManager already exists!");
            return;
        }
        Instance = this;

        if (PlayerPrefs.HasKey("Sound"))
        {
            volume = PlayerPrefs.GetFloat("Sound");
        }
        else
        {
            volume = .5f;
            PlayerPrefs.SetFloat("Sound", .5f);
        }
    }

    public AudioClip GetAudioClip(SoundType soundType, out float volume)
    {
        volume = this.volume;

        switch (soundType)
        {
            case SoundType.Steps:
                return Resources.Load<AudioClip>("steps");
            case SoundType.Attack:
                return Resources.Load<AudioClip>("attack");
            case SoundType.Fireball:
                return Resources.Load<AudioClip>("fireball");
            case SoundType.Cookie:
                return Resources.Load<AudioClip>("cookie");
        }


        return null;
    }
}
