using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    private Animator anim;
    public GameObject signSprite;
    private bool canPress;

    public Transform playerTrans;
    private PlayerInputControl inputControl;

    private IInteractable targetItem;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();

        inputControl = new PlayerInputControl();
        inputControl.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        inputControl.GamePlay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
        InputSystem.onActionChange -= OnActionChange;
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = false;
        }
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if(actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("Keyboard");
                    break;
                case XInputController:
                    anim.Play("Xbox");
                    break;
                default:
                    break;
            }
        }
    }


    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress&& GetComponent<AudioDefination>())
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>().PlayAudioClip();
        }
    }
}
