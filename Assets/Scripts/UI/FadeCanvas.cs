using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;

    public FadeEvent_SO fadeEvent;

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {

        fadeEvent.OnEventRaised -= OnFadeEvent;
    }
    private void OnFadeEvent(Color target,float duraction, bool fadeIn)
    {
        fadeImage.DOBlendableColor(target, duraction);
    }
}
