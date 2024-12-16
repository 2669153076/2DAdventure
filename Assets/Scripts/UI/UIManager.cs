using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CharacterEvent_SO characterEvent_SO;

    public PlayerStateBar playerStateBar;

    public SceneLoadEvent_SO sceneUnLoadEvent;

    public VoidEvent_SO loadDataEvent;
    public VoidEvent_SO gameOverEvent;

    public GameObject gameOverPanel;
    public GameObject restartBtn;

    public VoidEvent_SO backToMenuEvent;

    public GameObject mobileTouch;  //触摸面板

    public Button settingBtn;
    public GameObject pausePanel;

    public VoidEvent_SO pauseEvent;
    public FloatEvent_SO syncVolumeEvent;

    public Slider masterSlider;

    private void Awake()
    {
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif

        settingBtn.onClick.AddListener(TogglePausePanel);
    }


    private void OnEnable()
    {
        characterEvent_SO.OnEventRaised += OnHealthEvent;
        sceneUnLoadEvent.LoadRequestEvent += OnUnLoadRequestEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }


    private void OnDisable()
    {
        characterEvent_SO.OnEventRaised -= OnHealthEvent;
        sceneUnLoadEvent.LoadRequestEvent -= OnUnLoadRequestEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }


    private void OnHealthEvent(Character character)
    {
        playerStateBar.OnHealthChange(character.curHp / character.maxHp);
        playerStateBar.OnPowerChange(character);
    }
    private void OnUnLoadRequestEvent(GameScene_SO sceneToGo, Vector3 arg1, bool arg2)
    {
        playerStateBar.gameObject.SetActive(sceneToGo.sceneType == E_SceneType.Location);
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnSyncVolumeEvent(float amount)
    {
        masterSlider.value = (amount+80)/100;
    }
}
