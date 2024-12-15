using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追击
/// </summary>
public class BoarChaseState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("IsRun", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(E_NPCState.Patrol);
        }
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall))
        {

            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("IsRun", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }

}
