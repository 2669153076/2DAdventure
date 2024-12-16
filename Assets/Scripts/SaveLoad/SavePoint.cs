using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public SpriteRenderer spriteRenderer;
    public Sprite darkSprite;
    public Sprite lightSprite;

    private bool isDone;

    public GameObject lightObj;

    public VoidEvent_SO saveDataEvent;

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;

            lightObj.SetActive(true);
            this.gameObject.tag = "Untagged";
        }

        saveDataEvent.RaiseEvent();
    }


    private void OnEnable()
    {
        spriteRenderer.sprite = isDone?lightSprite:darkSprite;
        lightObj.SetActive(isDone);
    }
}
