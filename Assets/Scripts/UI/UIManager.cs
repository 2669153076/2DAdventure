using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CharacterEvent_SO characterEvent_SO;

    public PlayerStateBar playerStateBar;


    private void OnEnable()
    {
        characterEvent_SO.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {
        characterEvent_SO.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        playerStateBar.OnHealthChange(character.curHp / character.maxHp);
        playerStateBar.OnPowerChange(character);
    }
}
