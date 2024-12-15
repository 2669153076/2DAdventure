using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGMSource;
    public AudioSource FXSource;

    public PlayAudioEvent_SO BGMEvent;
    public PlayAudioEvent_SO FXEvent;

    private void OnEnable()
    {
        BGMEvent.OnEventRaised += OnBGMEvent;
        FXEvent.OnEventRaised += OnFXEvent;
    }
    private void OnDisable()
    {

        BGMEvent.OnEventRaised -= OnBGMEvent;
        FXEvent.OnEventRaised -= OnFXEvent;
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}
