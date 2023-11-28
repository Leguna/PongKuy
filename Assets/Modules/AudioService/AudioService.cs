using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class AudioService : SingletonMonoBehaviour<AudioService>
{
    public enum AudioType
    {
        Click,
        Hover,
        Wall,
        Paddle,
        Win,
    }

    [SerializeField] private AudioSource audioSource;

    private Dictionary<string, AudioClip> audioClips;

    private void LoadAudioResources()
    {
        var clickAudioClip = Resources.Load<AudioClip>("Audio/click");
        var hoverAudioClip = Resources.Load<AudioClip>("Audio/hover");
        var wallAudioClip = Resources.Load<AudioClip>("Audio/wall");
        var paddleAudioClip = Resources.Load<AudioClip>("Audio/paddle");
        var winAudioClip = Resources.Load<AudioClip>("Audio/win");

        audioClips = new Dictionary<string, AudioClip>
        {
            { "click", clickAudioClip },
            { "hover", hoverAudioClip },
            { "wall", wallAudioClip },
            { "paddle", paddleAudioClip },
            { "win", winAudioClip },
        };
    }

    public void Init()
    {
        TryGetComponent(out audioSource);
        if (audioSource == null) return;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        LoadAudioResources();
    }

    public void PlayAudio(AudioType audioType)
    {
        var audioName = audioType.ToString().ToLower();
        if (!audioClips.ContainsKey(audioName)) return;
        audioSource.PlayOneShot(audioClips[audioName]);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetVolume() => audioSource.volume;
}