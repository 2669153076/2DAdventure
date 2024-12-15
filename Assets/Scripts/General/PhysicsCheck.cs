using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理检测
/// 检测是否在地面、墙壁等等
/// </summary>
public class PhysicsCheck : MonoBehaviour
{
    public float checkRaduis;   //检测半径
    public Vector2 offsetPos;   //检测位置偏移值
    public LayerMask groundLayer; //被检测的层


    public bool isGround;   //是否处于地面


    [Header("墙壁检测")]
    public bool touchLeftWall;
    public bool touchRightWall;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask wallLayer;

    public bool manual; //手动
    private CapsuleCollider2D capsuleCollider;

    public bool isOnWall;
    public bool isPlayer;
    public PlayerController playerController;

    public Rigidbody2D rb;


    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2((capsuleCollider.bounds.size.x + capsuleCollider.offset.x )/ 2 , capsuleCollider.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x,rightOffset.y);
        }

        if(isPlayer)
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        if (isOnWall)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(offsetPos.x * transform.localScale.x, offsetPos.y), checkRaduis, groundLayer);
        }
        else
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(offsetPos.x * transform.localScale.x, 0), checkRaduis, groundLayer);
        }

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position+leftOffset,checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position+rightOffset,checkRaduis, groundLayer);

        if(isPlayer)
        {
            isOnWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y<0f;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + offsetPos, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
