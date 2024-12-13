using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl playerInputControl;
    private PhysicsCheck checkGround;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 inputDirection;

    public float speed = 5;

    public float jumpForce = 10;



    private void Awake()
    {
        playerInputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        checkGround = GetComponent<PhysicsCheck>();

        playerInputControl.GamePlay.Jump.started += Jump;
    }


    private void OnEnable()
    {
        playerInputControl.Enable();
    }

    private void OnDisable()
    {
        playerInputControl.Disable();
    }

    private void Update()
    {
        inputDirection = playerInputControl.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * Time.deltaTime * speed, rb.velocity.y);

        if (inputDirection.x < 0)
        {
            sr.flipX  = true;
        }else if (inputDirection.x > 0)
        {
            sr.flipX = false;
        }

    }

    /// <summary>
    /// 跳跃
    /// </summary>
    /// <param name="context"></param>
    private void Jump(InputAction.CallbackContext context)
    {
        if (checkGround.isGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   //向上添加瞬时的力
        }
    }
}
