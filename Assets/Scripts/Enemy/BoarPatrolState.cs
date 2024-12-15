﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡逻
/// </summary>
public class BoarPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        this.currentEnemy = enemy;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(E_NPCState.Chase);
        }

        if (!currentEnemy.physicsCheck.isGround ||(currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall))
        {

            currentEnemy.isWait = true;
            currentEnemy.animator.SetBool("IsWalk", false);
        }
        else
        {
            currentEnemy.animator.SetBool("IsWalk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {

        currentEnemy.animator.SetBool("IsWalk", false);
    }

}
