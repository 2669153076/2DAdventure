using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Sprite open;
    public Sprite close;
    public bool isDone;
    public void TriggerAction()
    {
        
    }

    private void OnEnable()
    {
            this.GetComponent<SpriteRenderer>().sprite=isDone?open:close;
    }

    private void OpenChest()
    {
        this.GetComponent<SpriteRenderer>().sprite = open;
        isDone = true;
        this.tag = "Untagged";
    }

    private void CloseChest()
    {
        this.GetComponent<SpriteRenderer>().sprite =  close;
        isDone = false;
        this.tag = "IInteractable";
    }
    }
