using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGMSource;
    public AudioSource FXSource;

    public PlayAudioEvent_SO BGMEvent;
    public PlayAudioEvent_SO FXEvent;

    public AudioMixer audioMixer;

    public FloatEvent_SO volumeEvent;

    public VoidEvent_SO pauseEvent;
    public FloatEvent_SO syncVolumeEvent;

    private void OnEnable()
    {
        BGMEvent.OnEventRaised += OnBGMEvent;
        FXEvent.OnEventRaised += OnFXEvent;
        volumeEvent.OnEventRaised += OnVolumeEvent;
        pauseEvent.OnEventRaised += OnPauseEvent;
    }
    private void OnDisable()
    {

        BGMEvent.OnEventRaised -= OnBGMEvent;
        FXEvent.OnEventRaised -= OnFXEvent;
        volumeEvent.OnEventRaised -= OnVolumeEvent;
        pauseEvent.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        float amount;
        audioMixer.GetFloat("MasterVolume", out amount);
        syncVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeEvent(float value)
    {
        audioMixer.SetFloat("MasterVolume", value * 100 - 80);
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
