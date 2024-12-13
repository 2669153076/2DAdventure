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

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + offsetPos, checkRaduis, groundLayer);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + offsetPos, checkRaduis);
    }
}
