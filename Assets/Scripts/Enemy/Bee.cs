using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }

    public override void Move()
    {
        
    }

    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, atkCheckLayer);
        if (obj)
        {
            attacker = obj.transform;
        }

        return obj;
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position , checkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position , patrolRadius);

    }

    /// <summary>
    /// 随机移动的点
    /// </summary>
    /// <returns></returns>
    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);
        return spawnPoint + new Vector3(targetX, targetY);

    }
}
