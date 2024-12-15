using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float normalSpeed;   //正常速度
    public float chaseSpeed;    //追击速度
    public float currentSpeed;
    public Vector3 faceDir;

    public Animator animator;
    public Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    public PhysicsCheck physicsCheck;

    public float waitTime = 1;
    public float waitTimeCounter;
    public bool isWait;

    public Transform attacker;

    public bool isHurt;
    public float hurtForce;

    public bool isDead;

    protected BaseState patrolState;    //巡逻
    protected BaseState currentState;
    protected BaseState chaseState; //追击
    protected BaseState skillState;

    [Header("追击检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask atkCheckLayer;

    [Header("玩家丢失计时")]
    public float lostTime;
    public float lostTimeCounter;

    public Vector3 spawnPoint;    //出生点

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        //waitTimeCounter = waitTime;

        spawnPoint = transform.position;
    }

    protected virtual void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    protected virtual void OnDisable()
    {
        currentState.OnExit();
    }

    protected virtual void Update()
    {
        //faceDir = spriteRenderer.flipX?Vector2.right:Vector2.left;
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        
        currentState.LogicUpdate();

        TimeCounter();

    }

    protected virtual void FixedUpdate()
    {
        currentState.PhysicsUpdate();

        if (!isHurt && !isDead && !isWait)
        {
            Move();
        }
    }

    public virtual void Move()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("NormalSnail_PreMove")&& !animator.GetCurrentAnimatorStateInfo(0).IsName("NormalSnail_Recover"))
        {
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }
    }

    public void TimeCounter()
    {
        if(isWait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                isWait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        if (!FoundPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        //else
        //{
        //    lostTimeCounter = lostTime;
        //}
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        this.attacker = attackTrans;

        //转身
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        isHurt = true;
        animator.SetTrigger("Hurt");

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y); //受击先将x方向的力置为0，防止冲刺时的力大于被击退的力，导致受击不会后退
        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(4/6f);  //图片张数除以帧数
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;   //设置层级，取消与玩家的碰撞 设置中取消player与ignore的碰撞
        animator.SetBool("IsDead", true);
        isDead = true;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 发现玩家
    /// 追击判断
    /// </summary>
    /// <returns></returns>
    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, atkCheckLayer);
    }

    public void SwitchState(E_NPCState state)
    {
        var newState = state switch
        {
            E_NPCState.Patrol => patrolState,
            E_NPCState.Chase=>chaseState,
            E_NPCState.Skill=>skillState,
            _=>null,
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
    }

    /// <summary>
    /// 随机移动的点
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }
}
