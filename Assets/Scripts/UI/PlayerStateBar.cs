using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image health;
    public Image healthDelay;
    public Image power;

    private bool isRecoverPower;
    private Character currentCharacter;

    private void Update()
    {
        if (healthDelay.fillAmount > health.fillAmount)
        {
            healthDelay.fillAmount-=Time.deltaTime;
        }
        if(isRecoverPower)
        {
            power.fillAmount = currentCharacter.currentPower/healthDelay.fillAmount;

            if(currentCharacter.currentPower / healthDelay.fillAmount > 1)
            {
                isRecoverPower = false;
                return;
            }
        }
    }

    public void OnHealthChange(float persentage)
    {
        health.fillAmount = persentage;
    }

    public void OnPowerChange(Character character)
    {
        isRecoverPower = true;
        this.currentCharacter = character;
    }
}
