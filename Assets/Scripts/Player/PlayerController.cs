using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl playerInputControl;
    private PhysicsCheck checkGround;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimator playerAnimator;

    public Vector2 inputDirection;

    private float speed;
    [SerializeField]private float runSpeed = 300;
    [SerializeField]private float walkSpeed = 150;

    public float jumpForce = 10;

    public bool isCrouch;

    private Vector2 originalOffset;
    private Vector2 originalSize;

    public bool isHurt;
    public float hurtForce; //受伤的力

    public bool isDead;

    public bool isAttack;

    public PhysicsMaterial2D normalMaterial;
    public PhysicsMaterial2D wallMaterial;

    public float jumpOnWallForce;

    public bool isWallJump;

    [Header("滑铲")]
    public float slideSpeed;
    public bool isSlide;
    public float slideDistance;
    public float slideCost; //滑铲消耗

    private Character character;

    public SceneLoadEvent_SO sceneLoadEvent;    
    public VoidEvent_SO afterSceneLoadEvent;
    public VoidEvent_SO loadDataEvent;

    public VoidEvent_SO backToMenuEvent;

    private void Awake()
    {
        playerInputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        checkGround = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<PlayerAnimator>();    

        playerInputControl.GamePlay.Jump.started += Jump;

        speed = runSpeed;
        playerInputControl.GamePlay.WalkButton.performed += ctx => { if (checkGround.isGround) { speed = walkSpeed; } };
        playerInputControl.GamePlay.WalkButton.canceled += ctx => { if (checkGround.isGround) { speed = runSpeed; } };

        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;

        playerInputControl.GamePlay.Attack.started += PlayerAttack;
        playerInputControl.GamePlay.Slide.started += Slide;

        character = GetComponent<Character>();

        playerInputControl.Enable();
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!checkGround.isGround)
        {
            return;
        }
        playerAnimator.PlayAttack();
        isAttack = true;
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadRequestEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;

    }

    private void OnDisable()
    {
        playerInputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadRequestEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

    private void Update()
    {
        inputDirection = playerInputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt&&!isAttack){
            Move();
        }
        if (isAttack)
        {
            rb.velocity = new Vector2(0, 0);
        }

    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        if (!isCrouch && !isAttack && !isWallJump)
        {
            rb.velocity = new Vector2(inputDirection.x * Time.deltaTime * speed, rb.velocity.y);
        }
        if (inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //sr.flipX  = true;
        }
        else if (inputDirection.x > 0)
        {
            //sr.flipX = false;

            transform.localScale = new Vector3(1, 1, 1);
        }

        #region 下蹲
        isCrouch = inputDirection.y < -0.5f && checkGround.isGround;

        if (isCrouch)
        {
            capsuleCollider.offset = new Vector2(-0.1f,0.8f);
            capsuleCollider.size = new Vector2(0.6f,1.6f);
        }
        else
        {
            capsuleCollider.offset = originalOffset;
            capsuleCollider.size = originalSize;
        }
        #endregion

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

            isSlide = false;
            StopAllCoroutines();
        }
        else if (checkGround.isOnWall)
        {
            //2f : 蹬墙跳向上方向增量，可修改
            rb.AddForce(new Vector2(-inputDirection.x,2f)*jumpOnWallForce, ForceMode2D.Impulse);
            isWallJump = true;
        }
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;

        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }


    public void PlayerDead()
    {
        isDead = true;
        playerInputControl.GamePlay.Disable();
    }

    private void CheckState()
    {
        capsuleCollider.sharedMaterial = checkGround.isGround ? normalMaterial : wallMaterial;

        if (checkGround.isOnWall)
        {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/2);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        if (isWallJump && rb.velocity.y < 0)
        {
            isWallJump= false;
        }
    }


    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide&&checkGround.isGround&&character.currentPower>=slideCost)
        {
            isSlide = true;
            var target = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
            gameObject.layer = LayerMask.NameToLayer("Enemy");

            StartCoroutine(TriggerSlide(target));

            character.OnSlide(slideCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if (!checkGround.isGround)
            {
                break;
            }
            if ((checkGround.touchLeftWall&&transform.localScale.x<0f) || (checkGround.touchRightWall&&transform.localScale.x>0f))
            {
                break;
            }

            rb.MovePosition(new Vector2(transform.position.x+transform.localScale.x*slideSpeed,transform.position.y));
        } while (Mathf.Abs(target.x - transform.position.x) > 0.1f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }



    /// <summary>
    /// 场景加载过程中不允许输入
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void OnLoadRequestEvent(GameScene_SO arg0, Vector3 arg1, bool arg2)
    {
        
        playerInputControl.GamePlay.Disable();
    }
    private void OnAfterSceneLoadEvent()
    {
        playerInputControl.GamePlay.Enable();
    }
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

}
