using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource cinemachineImpulse;

    public VoidEvent_SO cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }
    private void OnDisable()
    {

        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    private void Start()
    {
        GetNewCameraBounds();
    }

    void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if(obj == null)
        {
            return;
        }

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }



    private void OnCameraShakeEvent()
    {
        cinemachineImpulse.GenerateImpulse();
    }
}
